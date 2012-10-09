using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class DelegationViewModel
    {
        private Repositories.IDelegationRepository delegationRepository;

        public DelegationViewModel(Repositories.IDelegationRepository delegationRepository)
        {
            // TODO: Complete member initialization
            this.delegationRepository = delegationRepository;
            this.Users = delegationRepository.GetAllUsers(-1, -1);
        }

        public IEnumerable<string> Users { get; set; }
    }
}