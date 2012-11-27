using System.Collections.Generic;
using System.Linq;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class RelyingPartiesViewModel
    {
        IRelyingPartyRepository RelyingPartyRepository { get; set; }

        List<RelyingPartyViewModel> rps = new List<RelyingPartyViewModel>();
        public IEnumerable<RelyingPartyViewModel> RPs
        {
            get
            {
                return rps;
            }
        }

        public RelyingPartiesViewModel(IRelyingPartyRepository relyingPartyRepository)
        {
            RelyingPartyRepository = relyingPartyRepository;
            var query = RelyingPartyRepository.List(-1, -1);
            var items = query.Select(x => new RelyingPartyViewModel { ID = x.Id, DisplayName = x.Name, Enabled = x.Enabled });
            rps.AddRange(items);
        }

        internal void Update(IEnumerable<RelyingPartyViewModel> list)
        {
            foreach (var item in list)
            {
                var dbItem = this.RelyingPartyRepository.Get(item.ID);
                if (dbItem.Enabled != item.Enabled)
                {
                    dbItem.Enabled = item.Enabled;
                    this.RelyingPartyRepository.Update(dbItem);
                }
            }
        }
    }
}