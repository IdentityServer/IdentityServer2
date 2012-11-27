using System.Collections.Generic;
using System.Linq;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class RolesViewModel
    {
        private Repositories.IUserManagementRepository UserManagementRepository;
        public IEnumerable<RoleInputModel> Roles { get; set; }

        public RolesViewModel(Repositories.IUserManagementRepository UserManagementRepository)
        {
            this.UserManagementRepository = UserManagementRepository;
            this.Roles =
                UserManagementRepository
                    .GetRoles()
                    .Select(x => new RoleInputModel { Name = x })
                    .OrderBy(x=>x.CanDelete)
                    .ToArray();
        }
    }
}