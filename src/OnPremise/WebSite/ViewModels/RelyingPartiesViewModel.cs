using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.ViewModels
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
            var query = RelyingPartyRepository.List(-1, -1).OrderBy(x=>x.Name).Select(x => new RelyingPartyViewModel { ID = x.Id, DisplayName = x.Name, Enabled = x.Enabled });
            rps.AddRange(query);
        }
    }
}