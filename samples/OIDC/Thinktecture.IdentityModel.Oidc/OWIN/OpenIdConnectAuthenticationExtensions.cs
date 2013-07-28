using Owin;
using System;

namespace Thinktecture.IdentityModel.Oidc.OWIN
{
    public static class OpenIdConnectAuthenticationExtensions
    {
        public static IAppBuilder UseOpenIdConnectAuthentication(this IAppBuilder app, OpenIdConnectAuthenticationOptions options)
        {
            app.Use(typeof(OpenIdConnectAuthenticationMiddleware), options);
            return app;
        }

        public static IAppBuilder UseOpenIdConnectAuthentication(this IAppBuilder app, Action<OpenIdConnectAuthenticationOptions> configuration)
        {
            var options = new OpenIdConnectAuthenticationOptions();
            configuration(options);
            return UseOpenIdConnectAuthentication(app, options);
        }
    }
}
