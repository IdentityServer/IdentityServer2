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

        public UsersViewModel(Repositories.IUserManagementRepository UserManagementRepository, int currentPage, string filter)
        {
            Container.Current.SatisfyImportsOnce(this);

            this.UserManagementRepository = UserManagementRepository;
            this.Filter = filter;

            Init(currentPage, filter);
            if (TotalPages < CurrentPage)
            {
                Init(TotalPages, filter);
            }
        }

        private void Init(int currentPage, string filter)
        {
            if (currentPage <= 0) CurrentPage = 1;
            else CurrentPage = currentPage;

            int rows = 20;
            int startRow = (currentPage - 1) * rows;

            if (String.IsNullOrEmpty(filter))
            {
                int total;
                Users = UserManagementRepository.GetUsers(startRow, rows, out total);
                Total = total;
            }
            else
            {
                int total;
                Users = UserManagementRepository.GetUsers(filter, startRow, rows, out total);
                Total = total;
            }

            if (Total < rows)
            {
                Showing = Total;
            }
            else
            {
                Showing = rows;
            }
            UsersDeleteList = Users.Select(x => new UserDeleteModel { Username = x }).ToArray();

        }

        public IEnumerable<string> Users { get; set; }
        public UserDeleteModel[] UsersDeleteList {get;set;}
        public string Filter { get; set; }
        public int Total { get; set; }
        public int Showing { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages
        {
            get
            {
                if (Total <= 0 || Showing <= 0) return 1;
                return (int)Math.Ceiling((1.0*Total) / Showing);
            }
        }

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