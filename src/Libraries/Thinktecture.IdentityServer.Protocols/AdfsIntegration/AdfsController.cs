using System;
using System.ComponentModel.Composition;
using System.IdentityModel.Tokens;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web.Http;
using Thinktecture.IdentityModel.WSTrust;
using Thinktecture.IdentityServer.Protocols.OAuth2;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityModel.Tokens;
using System.Linq;
using System.IdentityModel.Selectors;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Claims;

namespace Thinktecture.IdentityServer.Protocols.AdfsIntegration
{
    public class AdfsController : ApiController
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public AdfsController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public AdfsController(IConfigurationRepository configurationRepository)
        {
            ConfigurationRepository = ConfigurationRepository;
        }

        public HttpResponseMessage Post(TokenRequest request)
        {
            Uri uri;
            if (string.IsNullOrEmpty(request.Scope) ||
                !Uri.TryCreate(request.Scope, UriKind.Absolute, out uri))
            {
                return OAuthErrorResponseMessage(OAuth2Constants.Errors.InvalidScope);
            }

            if (request.Grant_Type.Equals(OAuth2Constants.GrantTypes.Password))
            {
                return ProcessAuthenticationRequest(request);
            }

            return OAuthErrorResponseMessage(OAuth2Constants.Errors.UnsupportedGrantType);
        }

        private HttpResponseMessage ProcessAuthenticationRequest(TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName) ||
                string.IsNullOrEmpty(request.Password))
            {
                return OAuthErrorResponseMessage(OAuth2Constants.Errors.InvalidGrant);
            }

            GenericXmlSecurityToken token;
            try
            {
                token = AdfsBridge.Authenticate(request.UserName, request.Password, request.Scope, ConfigurationRepository.AdfsIntegration.UserNameEndpoint);
            }
            catch (Exception ex)
            {
                Tracing.Error("Error while communicating with ADFS: " + ex.ToString());
                return OAuthErrorResponseMessage(OAuth2Constants.Errors.InvalidGrant);
            }

            return CreateAuthenticationTokenResponse(token);
        }

        private HttpResponseMessage CreateAuthenticationTokenResponse(GenericXmlSecurityToken token)
        {
            var response = new TokenResponse();

            if (ConfigurationRepository.AdfsIntegration.CreateNewToken)
            {
                response = AdfsBridge.ConvertSamlToJwt(
                    token.ToSecurityToken(),
                    ConfigurationRepository.AdfsIntegration.IssuerSigningThumbprint,
                    ConfigurationRepository.AdfsIntegration.SymmetricSigningKey,
                    ConfigurationRepository.Global.IssuerUri);
            }
            else
            {
                response.AccessToken = token.TokenXml.OuterXml;
            }

            return Request.CreateResponse<TokenResponse>(HttpStatusCode.OK, response);
        }

        private HttpResponseMessage OAuthErrorResponseMessage(string error)
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                string.Format("{{ \"{0}\": \"{1}\" }}", OAuth2Constants.Errors.Error, error));
        }
    }
}
