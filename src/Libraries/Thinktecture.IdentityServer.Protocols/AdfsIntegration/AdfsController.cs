using System;
using System.ComponentModel.Composition;
using System.IdentityModel.Tokens;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityServer.Protocols.OAuth2;
using Thinktecture.IdentityServer.Repositories;

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
                Tracing.Error("Starting ADFS integration request with invalid scope: " + request.Scope);
                return OAuthErrorResponseMessage(OAuth2Constants.Errors.InvalidScope);
            }

            if (request.Grant_Type.Equals(OAuth2Constants.GrantTypes.Password))
            {
                return ProcessAuthenticationRequest(request);
            }
            else if (request.Grant_Type.Equals(OAuth2Constants.GrantTypes.Saml2))
            {
                return ProcessDelegationRequest(request);
            }

            Tracing.Error("Unsupported grant type: " + request.Grant_Type);
            return OAuthErrorResponseMessage(OAuth2Constants.Errors.UnsupportedGrantType);
        }

        #region Authentication
        private HttpResponseMessage ProcessAuthenticationRequest(TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName) ||
                string.IsNullOrEmpty(request.Password))
            {
                Tracing.Error("ADFS integration authentication request with empty username or password");
                return OAuthErrorResponseMessage(OAuth2Constants.Errors.InvalidGrant);
            }

            Tracing.Information("Starting ADFS integration authentication request for scope: " + request.Scope);

            GenericXmlSecurityToken token;
            try
            {
                Tracing.Information("Starting WS-Trust authentication request for user: " + request.UserName);

                token = AdfsBridge.Authenticate(
                    request.UserName, 
                    request.Password, 
                    request.Scope, 
                    new Uri(ConfigurationRepository.AdfsIntegration.AuthenticationEndpoint));
            }
            catch (Exception ex)
            {
                Tracing.Error("Error while communicating with ADFS: " + ex.ToString());
                return OAuthErrorResponseMessage(OAuth2Constants.Errors.InvalidGrant);
            }

            var response = CreateAuthenticationTokenResponse(token);
            Tracing.Information("ADFS integration authentication request successful");

            return response;
        }

        private HttpResponseMessage CreateAuthenticationTokenResponse(GenericXmlSecurityToken token)
        {
            var response = new TokenResponse();

            if (ConfigurationRepository.AdfsIntegration.PassThruAuthenticationToken)
            {
                response.AccessToken = token.TokenXml.OuterXml;
                response.ExpiresIn = (int)(token.ValidTo.Subtract(DateTime.UtcNow).TotalSeconds);
            }
            else
            {
                response = AdfsBridge.ConvertAuthenticationSamlToJwt(
                    token.ToSecurityToken(),
                    ConfigurationRepository.AdfsIntegration.IssuerThumbprint,
                    ConfigurationRepository.AdfsIntegration.SymmetricSigningKey,
                    ConfigurationRepository.Global.IssuerUri,
                    ConfigurationRepository.AdfsIntegration.AuthenticationTokenLifetime);
            }

            return Request.CreateResponse<TokenResponse>(HttpStatusCode.OK, response);
        }
        #endregion

        #region Delegation
        private HttpResponseMessage ProcessDelegationRequest(TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Assertion))
            {
                Tracing.Error("ADFS integration delegation request with empty assertion");
                return OAuthErrorResponseMessage(OAuth2Constants.Errors.InvalidGrant);
            }

            Tracing.Information("Starting ADFS integration delegation request for scope: " + request.Scope);

            var handlers = SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection();
            var reader = new XmlTextReader(new StringReader(request.Assertion));

            SecurityToken token;
            if (handlers.CanReadToken(reader))
            {
                token = handlers.ReadToken(reader);
            }
            else
            {
                Tracing.Error("Invalid SAML assertion: " + request.Assertion);
                return OAuthErrorResponseMessage(OAuth2Constants.Errors.InvalidGrant);
            }

            GenericXmlSecurityToken delegationToken;
            try
            {
                delegationToken = AdfsBridge.Delegate(
                    ConfigurationRepository.AdfsIntegration.DelegationEndpoint,
                    token,
                    request.Scope,
                    "bob",
                    "abc!123");
            }
            catch (Exception ex)
            {
                Tracing.Error("Error while communicating with ADFS: " + ex.ToString());
                return OAuthErrorResponseMessage(OAuth2Constants.Errors.InvalidGrant);
            }

            return CreateDelegationTokenResponse(delegationToken);
        }

        private HttpResponseMessage CreateDelegationTokenResponse(GenericXmlSecurityToken token)
        {
            var response = new TokenResponse();

            if (ConfigurationRepository.AdfsIntegration.PassThruDelegationToken)
            {
                response.AccessToken = token.TokenXml.OuterXml;
            }
            else
            {
                response = AdfsBridge.ConvertDelegationSamlToJwt(
                    token.ToSecurityToken(),
                    ConfigurationRepository.AdfsIntegration.IssuerThumbprint,
                    ConfigurationRepository.AdfsIntegration.SymmetricSigningKey,
                    ConfigurationRepository.Global.IssuerUri);
            }

            return Request.CreateResponse<TokenResponse>(HttpStatusCode.OK, response);
        }
        #endregion

        private HttpResponseMessage OAuthErrorResponseMessage(string error)
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                string.Format("{{ \"{0}\": \"{1}\" }}", OAuth2Constants.Errors.Error, error));
        }
    }
}
