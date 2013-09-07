using System.Collections.Generic;

namespace Thinktecture.IdentityServer.Repositories
{
    public interface IUserManagementRepository
    {
        void CreateUser(string userName, string password, string email = null);
        void DeleteUser(string userName);
        
        IEnumerable<string> GetUsers(int start, int count, out int totalCount);
        IEnumerable<string> GetUsers(string filter, int start, int count, out int totalCount);

        void SetPassword(string userName, string password);

        void SetRolesForUser(string userName, IEnumerable<string> roles);
        IEnumerable<string> GetRolesForUser(string userName);

        IEnumerable<string> GetRoles();
        void CreateRole(string roleName);
        void DeleteRole(string roleName);
    }
}
