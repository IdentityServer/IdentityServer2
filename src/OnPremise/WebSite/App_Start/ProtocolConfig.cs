using System.ServiceModel.Activation;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Thinktecture.IdentityModel.Tokens.Http;
using Thinktecture.IdentityServer.Protocols.OAuth2;
using Thinktecture.IdentityServer.Protocols.WSTrust;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.TokenService;

namespace Thinktecture.IdentityServer.Web
{
    public class ProtocolConfig
    {
        public static void RegisterProtocols(HttpConfiguration httpConfiguration, RouteCollection routes, IConfigurationRepository configuration, IUserRepository users, IRelyingPartyRepository relyingParties)
        {
            var basicAuthConfig = CreateBasicAuthConfig(users);
            var clientAuthConfig = CreateClientAuthConfig();

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

            // oauth2 endpoint
            if (configuration.OAuth2.Enabled)
            {
                // token endpoint
                routes.MapHttpRoute(
                    name: "oauth2token",
                    routeTemplate: "issue/oauth2/token",
                    defaults: new { controller = "OAuth2Token" },
                    constraints: null,
                    handler: new AuthenticationHandler(clientAuthConfig, httpConfiguration)
                );

                // authorize endpoint
                routes.MapRoute(
                    "oauth2authorize",
                    "issue/oauth2/authorize",
                    new { controller = "OAuth2Authorize", action = "index" }
                );
            }

            // simple http endpoint
            if (configuration.SimpleHttp.Enabled)
            {
                routes.MapHttpRoute(
                    name: "simplehttp",
                    routeTemplate: "issue/simple",
                    defaults: new { controller = "SimpleHttp" },
                    constraints: null,
                    handler: new AuthenticationHandler(basicAuthConfig, httpConfiguration)
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
            #endregion
        }

        public static AuthenticationConfiguration CreateBasicAuthConfig(IUserRepository userRepository)
        {
            var authConfig = new AuthenticationConfiguration
            {
                InheritHostClientIdentity = false,
                DefaultAuthenticationScheme = "Basic",
                SetNoRedirectMarker = true,
                ClaimsAuthenticationManager = new ClaimsTransformer()
            };

            authConfig.AddBasicAuthentication((userName, password) => userRepository.ValidateUser(userName, password));
            return authConfig;
        }

        public static AuthenticationConfiguration CreateClientAuthConfig()
        {
            var authConfig = new AuthenticationConfiguration
            {
                InheritHostClientIdentity = false,
                DefaultAuthenticationScheme = "Basic",
                SetNoRedirectMarker = true,
            };

            // accept arbitrary credentials on basic auth header,
            // validation will be done in the protocol endpoint
            authConfig.AddBasicAuthentication((id, secret) => true, retainPassword: true);
            return authConfig;
        }
    }
}