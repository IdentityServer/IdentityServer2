using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Web.Security;

namespace Thinktecture.IdentityServer.Repositories
{
    public class ProviderUserManagementRepository : IUserManagementRepository
    {
        public void CreateUser(string userName, string password, string email = null)
        {
            Membership.CreateUser(userName, password, email);
        }

        public void DeleteUser(string userName)
        {
            Membership.DeleteUser(userName);
        }

        public void SetRolesForUser(string userName, IEnumerable<string> roles)
        {
            var userRoles = Roles.GetRolesForUser(userName);

            if (userRoles.Length != 0)
            {
                Roles.RemoveUserFromRoles(userName, userRoles);
            }

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
            try
            {
                Roles.CreateRole(roleName);
            }
            catch (ProviderException)
            { }
        }

        public void DeleteRole(string roleName)
        {
            try
            {
                Roles.DeleteRole(roleName);
            }
            catch (ProviderException)
            { }
        }
    }
}
