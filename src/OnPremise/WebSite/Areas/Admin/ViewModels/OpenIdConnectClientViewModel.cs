using System.ComponentModel.Composition;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class OpenIdConnectClientViewModel
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }
        public OpenIdConnectClient Client { get; set; }

        public OpenIdConnectClientViewModel(Models.OpenIdConnectClient client)
        {
            Container.Current.SatisfyImportsOnce(this);
            this.Client = client;
        }

        public bool IsNew
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.Client.ClientId);
            }
        }

        public bool IsOAuthRefreshTokenEnabled
        {
            get
            {
                return !IsNew && Client.Flow == OpenIdConnectFlows.AuthorizationCode;
            }
        }
    }
}