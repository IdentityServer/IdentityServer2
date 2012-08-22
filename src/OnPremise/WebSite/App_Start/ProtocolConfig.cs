using System.ServiceModel.Activation;
using System.Web.Mvc;
using System.Web.Routing;
using Thinktecture.IdentityServer.Protocols.WSTrust;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.TokenService;
using System.Web.Http;

namespace Thinktecture.IdentityServer.Web
{
    public class ProtocolConfig
    {
        public static void RegisterProtocols(RouteCollection routes, IConfigurationRepository configuration)
        {
            #region Protocols
            // federation metadata
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

            // simple http web api implementation
            // todo: add config check
            //routes.MapHttpRoute(
            //    name: "simplehttp",
            //    routeTemplate: "issue/simplehttp",
            //    defaults: new { controller = "SimpleHttp2Controller" }
            //);

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