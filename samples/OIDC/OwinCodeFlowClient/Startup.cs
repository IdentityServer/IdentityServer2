using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using Thinktecture.IdentityModel.Oidc.OWIN;

namespace OwinDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ReturnPath = "http://foo.com/return"
                });


            // authentication
            //app.Use(typeof(AuthenticationMiddleware));
            //app.Use(typeof(SetPrincipalMiddleware));

            // web api
            //var httpConfig = new HttpConfiguration();
            //httpConfig.Routes.MapHttpRoute(
            //    "default",
            //    "api/{controller}");

            //app.UseWebApi(httpConfig);

            // plain http handler
            app.UseHandlerAsync((req, res) =>
            {
                res.ContentType = "text/plain";

                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                {
                    return res.WriteAsync("Hello " + Thread.CurrentPrincipal.Identity.Name);
                }
                else
                {
                    return res.WriteAsync("Hello stranger!");
                }
            });
        }
    }
}