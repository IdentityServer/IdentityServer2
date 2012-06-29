/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.Composition;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.ServiceModel;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.ActionResults;
using Thinktecture.IdentityServer.Web.Security;
using Thinktecture.IdentityServer.Web.ViewModels.OAuth2;
using Thinktecture.IdentityModel.Tokens;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    public class OAuth2Controller : Controller
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

        [HttpPost]
        public ActionResult Token(ResourceOwnerCredentialRequest request)
        {
            Tracing.Verbose("OAuth2 endpoint called.");

            if (!ConfigurationRepository.Endpoints.OAuth2)
            {
                Tracing.Error("OAuth2 endpoint called, but disabled in configuration");
                return new HttpNotFoundResult();
            }

            if (!ModelState.IsValid)
            {
                Tracing.Error("OAuth2 called with malformed request");
                return new HttpStatusCodeResult(400);
            }

            var auth = new AuthenticationHelper();

            Uri uri;
            if (!Uri.TryCreate(request.Scope, UriKind.Absolute, out uri))
            {
                Tracing.Error("OAuth2 endpoint called with malformed realm: " + request.Scope);
                return new HttpStatusCodeResult(400);
            }

            ClaimsPrincipal principal = null;
            if (auth.TryGetPrincipalFromOAuth2Request(Request, request, out principal))
            {
                if (!ClaimsAuthorize.CheckAccess(principal, Constants.Actions.Issue, Constants.Resources.OAuth2))
                {
                    Tracing.Error("User not authorized");
                    return new UnauthorizedResult("OAuth2", UnauthorizedResult.ResponseAction.Send401);
                }

                SecurityToken token;
                if (auth.TryIssueToken(new EndpointAddress(uri), principal, ConfigurationRepository.Configuration.HttpTokenType, out token))
                {
                    var jwt = token as JsonWebToken;
                    var response = new AccessTokenResponse
                    {
                        AccessToken = new JsonWebTokenHandler().WriteToken(jwt),
                        TokenType = TokenTypes.JsonWebToken,
                        ExpiresIn = ConfigurationRepository.Configuration.DefaultTokenLifetime * 60,
                    };

                    Tracing.Information("OAuth2 issue successful for user: " + request.UserName);
                    return new OAuth2AccessTokenResult(response);
                }

                return new HttpStatusCodeResult(400);
            }

            Tracing.Error("OAuth2 endpoint authentication failed for user: " + request.UserName);
            return new UnauthorizedResult("OAuth2", UnauthorizedResult.ResponseAction.Send401);
        }
    }
}
