using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Thinktecture.IdentityModel.Tokens.Http;
using MsftJwtIdentityModelExtensions;
using Thinktecture.IdentityModel;

namespace WebAPIApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var idsvrId = "http://identityserver.v2.thinktecture.com/samples";
            var cert = X509.LocalMachine.TrustedPeople.SubjectDistinguishedName.Find("CN=sts", false).Single();
            
            {
                var authConfig = new AuthenticationConfiguration();
                authConfig.AddMsftJsonWebToken(
                    idsvrId,
                    "http://localhost/rp-adfs-webapi1",
                    cert);

                var authHandler = new AuthenticationHandler(authConfig, config);

                config.Routes.MapHttpRoute(
                    name: "test1",
                    routeTemplate: "api/test1",
                    defaults: new { controller = "Test1" },
                    constraints: null,
                    handler: authHandler
                );
            }

            {
                var authConfig = new AuthenticationConfiguration();
                authConfig.AddMsftJsonWebToken(
                    idsvrId,
                    "http://localhost/rp-adfs-webapi2",
                    cert);

                var authHandler = new AuthenticationHandler(authConfig, config);

                config.Routes.MapHttpRoute(
                    name: "test2",
                    routeTemplate: "api/test2",
                    defaults: new { controller="Test2" },
                    constraints: null,
                    handler: authHandler
                );
            }
        }
    }
}
