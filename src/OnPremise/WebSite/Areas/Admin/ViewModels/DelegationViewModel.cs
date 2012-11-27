using System.Linq;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class DelegationViewModel
    {
        private Repositories.IDelegationRepository delegationRepository;

        public DelegationViewModel(Repositories.IDelegationRepository delegationRepository)
        {
            this.delegationRepository = delegationRepository;
        }

        DelegationUser[] users;
        public DelegationUser[] Users
        {
            get
            {
                if (users == null)
                {
                    users = delegationRepository
                        .GetAllUsers(-1, -1)
                        .Select(x => new DelegationUser { Username = x })
                        .ToArray();
                }
                return users;
            }
        }
    }

    public class DelegationUser
    {
        public string Username { get; set; }
        public bool Delete { get; set; }
    }
}