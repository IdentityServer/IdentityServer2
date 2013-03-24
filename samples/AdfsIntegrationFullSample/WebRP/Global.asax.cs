using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.IdentityModel.Tokens;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Thinktecture.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Web;

namespace WebRP
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //PassiveSessionConfiguration.ConfigureSessionCache(new EF.EFTokenCacheRepository());
            //PassiveSessionConfiguration.ConfigureMackineKeyProtectionForSessionTokens();
            //PassiveSessionConfiguration.ConfigureDefaultSessionDuration(TimeSpan.FromMinutes(1));
            //PassiveSessionConfiguration.ConfigurePersistentSessions(TimeSpan.FromDays(30));
            //FederatedAuthentication.SessionAuthenticationModule.IsReferenceMode
        }

        public override void Init()
        {
            //PassiveModuleConfiguration.CacheSessionsOnServer();
            //PassiveModuleConfiguration.EnableSlidingSessionExpirations();
            //PassiveModuleConfiguration.OverrideWSFedTokenLifetime();
            //PassiveModuleConfiguration.SuppressLoginRedirectsForApiCalls();
            //PassiveModuleConfiguration.SuppressSecurityTokenExceptions();
        }
    }
}