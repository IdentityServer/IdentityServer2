using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.Configuration)]
    public class OAuthRefreshTokenController : Controller
    {
        [Import]
        public IClientsRepository clientRepository { get; set; }

        public OAuthRefreshTokenController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public OAuthRefreshTokenController(IClientsRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public ActionResult Index(TokenSearchCriteria searchCriteria)
        {
            var vm = new OAuthRefreshTokenIndexViewModel(searchCriteria, clientRepository);
            
            return View(vm);
        }
    }
}
