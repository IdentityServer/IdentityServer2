using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Repositories
{
    // prototype
    public interface IUserManagementRepository
    {
        void CreateUser(string userName, string password, string displayName, string email);
        void DeleteUser(string userName);
        void SetRolesForUser(string userName, IEnumerable<string> roles);
        IEnumerable<string> GetRolesForUser(string userName);

        void CreateRole(string roleName);
        void DeleteRole(string roleName);
    }
}
