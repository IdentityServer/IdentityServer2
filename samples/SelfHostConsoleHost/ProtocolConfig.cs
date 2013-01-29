using System.Reflection;
using System.Web.Http;
using Thinktecture.IdentityModel.Http;
using Thinktecture.IdentityModel.Tokens.Http;
using Thinktecture.IdentityServer.Repositories;

namespace SelfHostConsoleHost
{
    public class ProtocolConfig
    {
        public static void RegisterProtocols(HttpConfiguration httpConfiguration, IConfigurationRepository configuration)
        {
            // necessary hack for now - until the DI implementation has been changed
            var a = Assembly.Load("Thinktecture.IdentityServer.Protocols");

            var clientAuthConfig = CreateClientAuthConfig();

            httpConfiguration.MessageHandlers.Add(new RequireHttpsHandler());

            if (configuration.OAuth2.Enabled)
            {        
                httpConfiguration.Routes.MapHttpRoute(
                    name: "oauth2token",
                    routeTemplate: Thinktecture.IdentityServer.Endpoints.Paths.OAuth2Token,
                    defaults: new { controller = "OAuth2Token" },
                    constraints: null,
                    handler: new AuthenticationHandler(clientAuthConfig, httpConfiguration)
                );
            }
        }

        public static AuthenticationConfiguration CreateClientAuthConfig()
        {
            var authConfig = new AuthenticationConfiguration
            {
                InheritHostClientIdentity = false,
                DefaultAuthenticationScheme = "Basic",
            };

            // accept arbitrary credentials on basic auth header,
            // validation will be done in the protocol endpoint
            authConfig.AddBasicAuthentication((id, secret) => true, retainPassword: true);

            return authConfig;
        }
    }
}