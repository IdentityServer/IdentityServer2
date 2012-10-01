using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace Thinktecture.IdentityServer.Repositories
{
    public class ProviderUserManagementRepository : ISimpleUserManagementRepository
    {
        public void CreateUser(string userName, string password, string email = null)
        {
            if (email != null)
            {
                Membership.CreateUser(userName, password);
            }
            else
            {
                Membership.CreateUser(userName, password, email);
            }
        }

        public void DeleteUser(string userName)
        {
            Membership.DeleteUser(userName);
        }

        public void SetRolesForUser(string userName, IEnumerable<string> roles)
        {
            var userRoles = Roles.GetRolesForUser(userName);
            Roles.RemoveUserFromRoles(userName, userRoles);

            Roles.AddUserToRoles(userName, roles.ToArray());
        }

        public IEnumerable<string> GetRolesForUser(string userName)
        {
            return Roles.GetRolesForUser(userName);
        }

        public IEnumerable<string> GetRoles()
        {
            return Roles.GetAllRoles();
        }

        public void CreateRole(string roleName)
        {
            Roles.CreateRole(roleName);
        }

        public void DeleteRole(string roleName)
        {
            Roles.DeleteRole(roleName);
        }
    }
}
