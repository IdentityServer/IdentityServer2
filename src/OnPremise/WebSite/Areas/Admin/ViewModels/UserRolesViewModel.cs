using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class UserRolesViewModel
    {
        private Repositories.IUserManagementRepository userManagementRepository;
        public string Username { get; set; }
        public IEnumerable<string> AllRoles { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public bool IsInRole(string role)
        {
            return Roles.Contains(role);
        }

        public UserRolesViewModel(Repositories.IUserManagementRepository userManagementRepository, string username)
        {
            this.userManagementRepository = userManagementRepository;
            this.Username = username;
            this.AllRoles = this.userManagementRepository.GetRoles();
            this.Roles = this.userManagementRepository.GetRolesForUser(this.Username);
        }


    }

    public class UserRolesInputModel
    {
        public string Username { get; set; }

    }
    
    public class UserRoleAssignment
    {
        public string Role { get; set; }
        public bool InRole { get; set; }
    }
}