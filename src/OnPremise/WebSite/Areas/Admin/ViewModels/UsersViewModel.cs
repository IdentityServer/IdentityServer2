using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class UsersViewModel
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        private Repositories.IUserManagementRepository UserManagementRepository;

        public UsersViewModel(Repositories.IUserManagementRepository UserManagementRepository, string filter)
        {
            Container.Current.SatisfyImportsOnce(this);

            this.UserManagementRepository = UserManagementRepository;
            this.Filter = filter;

            if (String.IsNullOrEmpty(filter))
            {
                Users = UserManagementRepository.GetUsers();
                Total = Showing = Users.Count();
            }
            else
            {
                Users = UserManagementRepository.GetUsers(filter);
                Total = UserManagementRepository.GetUsers().Count();
                Showing = Users.Count();
            }

            UsersDeleteList = Users.Select(x => new UserDeleteModel { Username = x }).ToArray();
        }

        public IEnumerable<string> Users { get; set; }
        public UserDeleteModel[] UsersDeleteList {get;set;}
        public string Filter { get; set; }
        public int Total { get; set; }
        public int Showing { get; set; }

        public bool IsProfileEnabled
        {
            get
            {
                return System.Web.Profile.ProfileManager.Enabled;
            }
        }
        
        public bool IsOAuthRefreshTokenEnabled
        {
            get
            {
                return ConfigurationRepository.OAuth2.Enabled &&
                    (ConfigurationRepository.OAuth2.EnableCodeFlow || ConfigurationRepository.OAuth2.EnableResourceOwnerFlow);
            }
        }
    }

    public class UserDeleteModel
    {
        public string Username { get; set; }
        public bool Delete { get; set; }
    }
}