using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class UserRolesViewModel
    {
        private Repositories.IUserManagementRepository userManagementRepository;
        public string Username { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public bool IsUserInRole(string role)
        {
            return UserRoles.Contains(role);
        }
        public UserRoleAssignment[] RoleAssignments
        {
            get
            {
                var allRoles = this.userManagementRepository.GetRoles();
                return (from role in allRoles
                        select new UserRoleAssignment
                        {
                            Role = role,
                            InRole = IsUserInRole(role)
                        }).ToArray();
            }
        }

        public UserRolesViewModel(Repositories.IUserManagementRepository userManagementRepository, string username)
        {
            this.userManagementRepository = userManagementRepository;
            this.Username = username;
            this.UserRoles = this.userManagementRepository.GetRolesForUser(this.Username);
        }
    }

    public class UserRoleAssignment
    {
        [Required]
        public string Role { get; set; }
        public bool InRole { get; set; }
    }
}