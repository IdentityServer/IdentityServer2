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
                Total = Showing;
            }
            else
            {
                Users = UserManagementRepository.GetUsers(filter);
                Total = UserManagementRepository.GetUsers().Count();
            }
        }

        public IEnumerable<string> Users { get; set; }
        public UserDeleteModel[] UsersDeleteList
        {
            get
            {
                return Users.Select(x => new UserDeleteModel { Username = x }).ToArray();
            }
        }

        public string Filter { get; set; }
        public int Total { get; set; }
        public int Showing
        {
            get
            {
                return Users.Count();
            }
        }
    }

    public class UserDeleteModel
    {
        public string Username { get; set; }
        public bool Delete { get; set; }
    }
}