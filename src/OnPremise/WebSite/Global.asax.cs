using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IdentityModel.Services;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.TokenService;

namespace Thinktecture.IdentityServer.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        protected void Application_Start()
        {
            SetupCompositionContainer();
            Container.Current.SatisfyImportsOnce(this);

            FederatedAuthentication.FederationConfigurationCreated += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationRepository.SigningCertificate.SubjectDistinguishedName))
                {
                    e.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers.AddOrReplace(
                        new X509CertificateSessionSecurityTokenHandler(ConfigurationRepository.SigningCertificate.Certificate));
                }
            };

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var context = (sender as HttpApplication).Context;

            if (context.Response.StatusCode == 401)
            {
                var noRedirect = context.Items["NoRedirect"];

                if (noRedirect == null)
                {
                    var route = new RouteValueDictionary(new Dictionary<string, object>
                        {
                            { "Controller", "Account" },
                            { "Action", "SignIn" },
                            { "ReturnUrl", HttpUtility.UrlEncode(context.Request.RawUrl, context.Request.ContentEncoding) }
                        });

                    Response.RedirectToRoute(route);
                }
            }
        }

        private void SetupCompositionContainer()
        {
            Container.Current = new CompositionContainer(new RepositoryExportProvider());
        }
    }
}