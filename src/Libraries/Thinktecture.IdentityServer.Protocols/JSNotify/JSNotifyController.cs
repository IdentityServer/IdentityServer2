///*
// * Copyright (c) Dominick Baier.  All rights reserved.
// * see license.txt
// */

//using System;
//using System.ComponentModel.Composition;
//using System.IdentityModel.Protocols.WSTrust;
//using System.Security.Claims;
//using System.ServiceModel;
//using System.Threading;
//using System.Web.Mvc;
//using Thinktecture.IdentityModel.Constants;
//using Thinktecture.IdentityServer.Repositories;

//namespace Thinktecture.IdentityServer.Protocols.JSNotify
//{
//    [ClaimsAuthorize(Constants.Actions.Issue, Constants.Resources.JSNotify)]
//    public class JSNotifyController : Controller
//    {
//        [Import]
//        public IConfigurationRepository ConfigurationRepository { get; set; }

//        public JSNotifyController()
//        {
//            Container.Current.SatisfyImportsOnce(this);
//        }

//        public JSNotifyController(IConfigurationRepository configurationRepository)
//        {
//            ConfigurationRepository = configurationRepository;
//        }

//        public ActionResult Issue(string realm, string tokenType)
//        {
//            Tracing.Verbose("JSNotify endpoint called.");

//            //if (!ConfigurationRepository.Endpoints.JSNotify)
//            //{
//            //    Tracing.Warning("JSNotify endpoint called, but disabled in configuration");
//            //    return new HttpNotFoundResult();
//            //}

//            Tracing.Information("JSNotify endpoint called for realm: " + realm);

//            if (tokenType == null)
//            {
//                tokenType = ConfigurationRepository.Global.DefaultHttpTokenType;
//            }

//            Tracing.Information("Token type: " + tokenType);

//            Uri uri;
//            if (!Uri.TryCreate(realm, UriKind.Absolute, out uri))
//            {
//                Tracing.Error("Realm parameter is malformed.");
//                return new HttpStatusCodeResult(400);
//            }

//            var endpoint = new EndpointReference(realm);
//            var auth = new AuthenticationHelper();

//            TokenResponse response;
//            var sts = new STS();
//            if (sts.TryIssueToken(endpoint, Thread.CurrentPrincipal as ClaimsPrincipal, tokenType, out response))
//            {
//                var jsresponse = new AccessTokenResponse
//                {
//                    AccessToken = response.TokenString,
//                    TokenType = response.TokenType,
//                    ExpiresIn = ConfigurationRepository.Global.DefaultTokenLifetime * 60
//                };

//                Tracing.Information("JSNotify issue successful for user: " + User.Identity.Name);
//                return new JSNotifyResult(jsresponse);
//            }
//            else
//            {
//                return new HttpStatusCodeResult(400);
//            }
//        }
//    }
//}
