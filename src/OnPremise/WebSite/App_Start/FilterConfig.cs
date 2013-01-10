using System.Web.Mvc;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.GlobalFilter;

namespace Thinktecture.IdentityServer.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters, IConfigurationRepository configuration)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new GlobalViewModelFilter());
            filters.Add(new SslRedirectFilter(configuration.Global.HttpsPort));
            filters.Add(new InitialConfigurationFilter());
        }
    }
}