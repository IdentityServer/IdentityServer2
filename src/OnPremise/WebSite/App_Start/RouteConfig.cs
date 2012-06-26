using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Thinktecture.IdentityServer.TokenService;

namespace Thinktecture.IdentityServer.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "FederationMetadata",
                "FederationMetadata/2007-06/FederationMetadata.xml",
                new { controller = "FederationMetadata", action = "Generate" }
            );

            routes.MapRoute(
                "RelyingPartiesAdmin",
                "admin/relyingparties/{action}/{id}",
                new { controller = "RelyingPartiesAdmin", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ClientCertificatesAdmin",
                "admin/clientcertificates/{action}/{userName}",
                new { controller = "ClientCertificatesAdmin", action = "Index", userName = UrlParameter.Optional }
            );

            routes.MapRoute(
                "DelegationAdmin",
                "admin/delegation/{action}/{userName}",
                new { controller = "DelegationAdmin", action = "Index", userName = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new { controller = "^(?!issue).*" }
            );

            // ws-federation (mvc)
            routes.MapRoute(
                "wsfederation",
                "issue/wsfed",
                new { controller = "WSFederation", action = "issue" }
            );

            // jsnotify (mvc)
            routes.MapRoute(
                "jsnotify",
                "issue/jsnotify",
                new { controller = "JSNotify", action = "issue" }
            );

            // simple http (mvc)
            routes.MapRoute(
                "simplehttp",
                "issue/simple",
                new { controller = "SimpleHttp", action = "issue" }
            );

            // oauth wrap (mvc)
            routes.MapRoute(
                "wrap",
                "issue/wrap",
                new { controller = "Wrap", action = "issue" }
            );

            // oauth2 (mvc)
            routes.MapRoute(
                "oauth2",
                "issue/oauth2/{action}",
                new { controller = "OAuth2", action = "token" }
            );

            // ws-trust (wcf)
            routes.Add(new ServiceRoute(
                "issue/wstrust",
                new TokenServiceHostFactory(),
                typeof(TokenServiceConfiguration)));
        }
    }
}