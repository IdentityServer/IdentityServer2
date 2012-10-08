using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class UsersViewModel
    {
        private Repositories.IUserManagementRepository UserManagementRepository;

        public UsersViewModel(Repositories.IUserManagementRepository UserManagementRepository, string filter)
        {
            // TODO: Complete member initialization
            this.UserManagementRepository = UserManagementRepository;
            this.Filter = filter;

            if (String.IsNullOrEmpty(filter))
            {
                Users = UserManagementRepository.GetUsers();
            }
            else
            {
                Users = UserManagementRepository.GetUsers(filter);
            }
        }

        public IEnumerable<string> Users { get; set; }
        public string Filter { get; set; }

    }
}