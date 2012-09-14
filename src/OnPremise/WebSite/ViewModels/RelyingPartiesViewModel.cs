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

        //List<RelyingPartyViewModel> rps = new List<RelyingPartyViewModel>();
        //public IEnumerable<RelyingPartyViewModel> RPs
        //{
        //    get
        //    {
        //        return rps;
        //    }
        //}

        //public RelyingPartiesViewModel(IRelyingPartyRepository relyingPartyRepository)
        //{
        //    RelyingPartyRepository = relyingPartyRepository;
        //    RelyingPartyRepository.List().Select(new RelyingPartyViewModel{
        //}
    }
}