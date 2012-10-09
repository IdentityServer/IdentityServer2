using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    public class DelegationController : Controller
    {
        [Import]
        public IDelegationRepository delegationRepository { get; set; }
        
        public DelegationController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public DelegationController(IDelegationRepository delegationRepository)
        {
            this.delegationRepository = delegationRepository;
        }
        
        public ActionResult Index()
        {
            var vm = new DelegationViewModel(this.delegationRepository);
            return View("Index", vm);
        }

    }
}
