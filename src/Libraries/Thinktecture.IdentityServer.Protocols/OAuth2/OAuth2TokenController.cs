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
using Thinktecture.IdentityModel.Authorization;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Protocols.OAuth2
{
    public class OAuth2TokenController : ApiController
    {
        [Import]
        public IUserRepository UserRepository { get; set; }

        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public OAuth2TokenController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public OAuth2TokenController(IUserRepository userRepository, IConfigurationRepository configurationRepository)
        {
            UserRepository = userRepository;
            ConfigurationRepository = configurationRepository;
        }

        public HttpResponseMessage Post(TokenRequest tokenRequest)
        {
            Tracing.Information("OAuth2 endpoint called.");

            var tokenType = ConfigurationRepository.Global.DefaultHttpTokenType;

            EndpointReference appliesTo;
            try
            {
                appliesTo = new EndpointReference(tokenRequest.Scope);
                Tracing.Information("OAuth2 endpoint called for scope: " + tokenRequest.Scope);
            }
            catch
            {
                Tracing.Error("Malformed scope: " + tokenRequest.Scope);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "malformed scope name.");
            }

            // check grant type
            if (string.Equals(tokenRequest.GrantType, OAuth2Constants.GrantTypes.Password, System.StringComparison.Ordinal))
            {
                return ProcessResourceOwnerCredentialRequest(tokenRequest.UserName, tokenRequest.Password, appliesTo, tokenType);
            }

            Tracing.Error("invalid grant type: " + tokenRequest.GrantType);
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid grant type.");
        }

        private HttpResponseMessage ProcessResourceOwnerCredentialRequest(string userName, string password, EndpointReference appliesTo, string tokenType)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                Tracing.Error("Missing username: " + appliesTo.Uri.AbsoluteUri);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "missing user name.");
            }

            var auth = new AuthenticationHelper();
            ClaimsPrincipal principal;
            if (UserRepository.ValidateUser(userName, password))
            {
                principal = auth.CreatePrincipal(userName, "OAuth2");

                if (!ClaimsAuthorization.CheckAccess(principal, Constants.Actions.Issue, Constants.Resources.OAuth2))
                {
                    return UnauthorizedResponse(userName);
                }
            }
            else
            {
                return UnauthorizedResponse(userName);
            }

            var sts = new STS();
            TokenResponse tokenResponse;
            if (sts.TryIssueToken(appliesTo, principal, tokenType, out tokenResponse))
            {
                var resp = Request.CreateResponse<TokenResponse>(HttpStatusCode.OK, tokenResponse);
                return resp;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid request.");
            }
        }

        private HttpResponseMessage UnauthorizedResponse(string userName)
        {
            Tracing.Error("OAuth2 endpoint authorization failed for user: " + userName);

            // todo: improve
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[Thinktecture.IdentityModel.Constants.Internal.NoRedirectLabel] = true;
            }

            return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "unauthorized.");
        }
    }
}
