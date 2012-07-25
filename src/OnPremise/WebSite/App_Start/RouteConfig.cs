using System.ServiceModel.Activation;
using System.Web.Mvc;
using System.Web.Routing;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.TokenService;

namespace Thinktecture.IdentityServer.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes, IConfigurationRepository configuration)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Administration & Configuration
            routes.MapRoute(
                "InitialConfiguration",
                "initialconfiguration",
                new { controller = "InitialConfiguration", action = "Index" }
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
            #endregion

            #region Main UI
            routes.MapRoute(
                "Account",
                "account/{action}",
                new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Home",
                "{action}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            #endregion
           
            #region Protocols
            if (configuration.FederationMetadata.Enabled)
            {
                routes.MapRoute(
                    "FederationMetadata",
                    "FederationMetadata/2007-06/FederationMetadata.xml",
                    new { controller = "FederationMetadata", action = "Generate" }
                );
            }

            // ws-federation
            if (configuration.WSFederation.Enabled && configuration.WSFederation.EnableAuthentication)
            {
                routes.MapRoute(
                    "wsfederation",
                    "issue/wsfed",
                    new { controller = "WSFederation", action = "issue" }
                );
            }

            // ws-federation HRD
            if (configuration.WSFederation.Enabled && configuration.WSFederation.EnableFederation)
            {
                routes.MapRoute(
                    "hrd",
                    "issue/hrd",
                    new { controller = "Hrd", action = "issue" }
                );
            }

            // oauth2
            if (configuration.OAuth2.Enabled)
            {
                routes.MapRoute(
                    "oauth2",
                    "issue/oauth2/{action}",
                    new { controller = "OAuth2", action = "token" }
                );
            }

            // ws-trust
            if (configuration.WSTrust.Enabled)
            {
                routes.Add(new ServiceRoute(
                    "issue/wstrust",
                    new TokenServiceHostFactory(),
                    typeof(TokenServiceConfiguration))
                );
            }


            //// jsnotify (mvc)
            //routes.MapRoute(
            //    "jsnotify",
            //    "issue/jsnotify",
            //    new { controller = "JSNotify", action = "issue" }
            //);

            //// simple http (mvc)
            //routes.MapRoute(
            //    "simplehttp",
            //    "issue/simple",
            //    new { controller = "SimpleHttp", action = "issue" }
            //);

            //// oauth wrap (mvc)
            //routes.MapRoute(
            //    "wrap",
            //    "issue/wrap",
            //    new { controller = "Wrap", action = "issue" }
            //);
            #endregion
        }

    }
}