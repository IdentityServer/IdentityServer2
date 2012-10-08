using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Repositories
{
    public interface IUserManagementRepository
    {
        void CreateUser(string userName, string password, string email = null);
        void DeleteUser(string userName);
        IEnumerable<string> GetUsers();
        IEnumerable<string> GetUsers(string filter);

        void SetRolesForUser(string userName, IEnumerable<string> roles);
        IEnumerable<string> GetRolesForUser(string userName);

        IEnumerable<string> GetRoles();
        void CreateRole(string roleName);
        void DeleteRole(string roleName);
    }
}
