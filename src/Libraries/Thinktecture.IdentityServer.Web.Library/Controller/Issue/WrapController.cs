///*
// * Copyright (c) Dominick Baier.  All rights reserved.
// * see license.txt
// */

//using System;
//using System.ComponentModel.Composition;
//using System.Security.Claims;
//using System.ServiceModel;
//using System.Web.Mvc;
//using Thinktecture.IdentityModel.Constants;
//using Thinktecture.IdentityServer.Repositories;
//using Thinktecture.IdentityServer.Web.ActionResults;
//using Thinktecture.IdentityServer.Web.Security;

//namespace Thinktecture.IdentityServer.Web.Controllers.Issue
//{
//    public class WrapController : Controller
//    {
//        [Import]
//        public IConfigurationRepository ConfigurationRepository { get; set; }

//        public WrapController()
//        {
//            Container.Current.SatisfyImportsOnce(this);
//        }

//        public WrapController(IConfigurationRepository configurationRepository)
//        {
//            ConfigurationRepository = configurationRepository;
//        }

//        [HttpPost]
//        public ActionResult Issue()
//        {
//            Tracing.Verbose("OAuth WRAP endpoint called.");

//            if (!ConfigurationRepository.Endpoints.OAuthWRAP)
//            {
//                Tracing.Error("OAuth WRAP endpoint is disabled in configuration");
//                return new HttpNotFoundResult();
//            }

//            var scope = Request.Form["wrap_scope"];

//            if (string.IsNullOrWhiteSpace(scope))
//            {
//                Tracing.Error("OAuth WRAP endpoint called with empty realm.");
//                return new HttpStatusCodeResult(400);
//            }

//            Uri uri;
//            if (!Uri.TryCreate(scope, UriKind.Absolute, out uri))
//            {
//                Tracing.Error("OAuth WRAP endpoint called with malformed realm: " + scope);
//                return new HttpStatusCodeResult(400);
//            }

//            Tracing.Information("OAuth WRAP endpoint called with realm: " + scope);

//            var endpoint = new EndpointAddress(uri);
//            var auth = new AuthenticationHelper();

//            ClaimsPrincipal principal;
//            if (!auth.TryGetPrincipalFromWrapRequest(Request, out principal))
//            {
//                Tracing.Error("Authentication failed");
//                return new UnauthorizedResult("WRAP", UnauthorizedResult.ResponseAction.Send401);
//            }

//            if (!ClaimsAuthorize.CheckAccess(principal, Constants.Actions.Issue, Constants.Resources.WRAP))
//            {
//                Tracing.Error("User not authorized");
//                return new UnauthorizedResult("WRAP", UnauthorizedResult.ResponseAction.Send401);
//            }

//            TokenResponse response;
//            if (auth.TryIssueToken(endpoint, principal, TokenTypes.SimpleWebToken, out response))
//            {
//                return new WrapResult(response);
//            }
//            else
//            {
//                return new HttpStatusCodeResult(400);
//            }
//        }
//    }
//}
