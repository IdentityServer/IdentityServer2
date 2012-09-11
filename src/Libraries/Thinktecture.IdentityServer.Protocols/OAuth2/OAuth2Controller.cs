/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.Composition;
using System.IdentityModel.Protocols.WSTrust;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Thinktecture.IdentityModel.Tokens.Http;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Protocols.OAuth2
{
    public class OAuth2Controller : ApiController
    {
        [Import]
        public IUserRepository UserRepository { get; set; }

        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public OAuth2Controller()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public OAuth2Controller(IUserRepository userRepository, IConfigurationRepository configurationRepository)
        {
            UserRepository = userRepository;
            ConfigurationRepository = configurationRepository;
        }

        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            Tracing.Information("OAuth2 endpoint called.");

            var tokenType = ConfigurationRepository.Global.DefaultHttpTokenType;
            var tokenRequest = ResourceOwnerCredentialRequest.Parse(request.Content.ReadAsFormDataAsync().Result);

            // todo: check grant_type

            if (string.IsNullOrWhiteSpace(tokenRequest.UserName))
            {
                Tracing.Error("Missung username: " + tokenRequest.Scope);
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, "missing user name.");
            }

            EndpointReference appliesTo;
            try
            {
                appliesTo = new EndpointReference(tokenRequest.Scope);
                Tracing.Information("OAuth2 endpoint called for scope: " + tokenRequest.Scope);
            }
            catch
            {
                Tracing.Error("Malformed scope: " + tokenRequest.Scope);
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, "malformed scope name.");
            }

            var auth = new AuthenticationHelper();
            ClaimsPrincipal principal;
            if (UserRepository.ValidateUser(tokenRequest.UserName, tokenRequest.Password))
            {
                principal = auth.CreatePrincipal(tokenRequest.UserName, "OAuth2");
            }
            else
            {
                Tracing.Error("OAuth2 endpoint authentication failed for user: " + tokenRequest.UserName);
                
                // todo: improve
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items[Thinktecture.IdentityModel.Constants.Internal.NoRedirectLabel] = true;
                }
                
                return request.CreateErrorResponse(HttpStatusCode.Unauthorized, "unauthorized.");
            }

            var sts = new STS();
            TokenResponse tokenResponse;
            if (sts.TryIssueToken(appliesTo, principal, tokenType, out tokenResponse))
            {
                var resp = request.CreateResponse<TokenResponse>(HttpStatusCode.OK, tokenResponse);
                return resp;
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid request.");
            }
        }
    }
}
