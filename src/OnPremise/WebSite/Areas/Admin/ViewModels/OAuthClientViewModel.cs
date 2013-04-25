using System.ComponentModel.Composition;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class OAuthClientViewModel
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }
        public Client Client { get; set; }

        public OAuthClientViewModel(Models.Client client)
        {
            Container.Current.SatisfyImportsOnce(this);
            this.Client = client;
        }

        public bool IsNew
        {
            get
            {
                return this.Client.ID == 0;
            }
        }

        public bool IsOAuthRefreshTokenEnabled
        {
            get
            {
                return !IsNew && 
                    (Client.AllowCodeFlow || Client.AllowResourceOwnerFlow) &&
                    ConfigurationRepository.OAuth2.Enabled &&
                    (ConfigurationRepository.OAuth2.EnableCodeFlow || ConfigurationRepository.OAuth2.EnableResourceOwnerFlow);
            }
        }
    }
}