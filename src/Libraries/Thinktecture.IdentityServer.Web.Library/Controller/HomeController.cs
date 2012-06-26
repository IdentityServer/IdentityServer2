/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    public class HomeController : Controller
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public HomeController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public HomeController(IConfigurationRepository configuration)
        {
            ConfigurationRepository = configuration;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AppIntegration()
        {
            var endpoints = Endpoints.Create(
                               HttpContext.Request.Headers["Host"],
                               HttpContext.Request.ApplicationPath,
                               ConfigurationRepository.Endpoints.HttpPort,
                               ConfigurationRepository.Endpoints.HttpsPort);

            var list = new Dictionary<string, string>
            {
                { "WS-Federation", endpoints.WSFederation.AbsoluteUri },
                { "WS-Federation metadata", endpoints.WSFederationMetadata.AbsoluteUri },
                
                { "WS-Trust mixed (UserName)", endpoints.WSTrustMixedUserName.AbsoluteUri },
                { "WS-Trust mixed (Certificate)", endpoints.WSTrustMixedCertificate.AbsoluteUri },
                { "WS-Trust message (UserName)", endpoints.WSTrustMessageUserName.AbsoluteUri },
                { "WS-Trust message (Certificate)", endpoints.WSTrustMessageCertificate.AbsoluteUri },
                { "WS-Trust metadata", endpoints.WSTrustMex.AbsoluteUri },

                { "WRAP", endpoints.Wrap.AbsoluteUri },
                { "OAuth2", endpoints.OAuth2.AbsoluteUri },
                { "JSNotify", endpoints.JSNotify.AbsoluteUri },
                { "Simple HTTP", endpoints.SimpleHttp.AbsoluteUri },
            };

            return View(list);
        }
    }
}
