using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class UsersViewModel
    {
        private Repositories.IUserManagementRepository UserManagementRepository;

        public UsersViewModel(Repositories.IUserManagementRepository UserManagementRepository)
        {
            // TODO: Complete member initialization
            this.UserManagementRepository = UserManagementRepository;
        }

        //public IEnumerable<UserInputModel> Search(string filter)
        //{
        //    var query = 
        //        from u in this.UserManagementRepository.
        //}
    }
}