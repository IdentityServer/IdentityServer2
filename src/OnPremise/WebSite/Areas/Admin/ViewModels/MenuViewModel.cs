using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class MenuViewModel
    {
        public MenuViewModel(IConfigurationRepository configuration)
        {
            this.ShowOAuthClients = configuration.OAuth2.Enabled;
            this.ShowOAuthTokens = configuration.OAuth2.Enabled && configuration.OAuth2.EnableCodeFlow;
        }

        public bool ShowOAuthClients { get; private set; }
        public bool ShowOAuthTokens { get; private set; }
    }
}