using System;
using System.Web.Mvc;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.Authorization.Mvc;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.Configuration)]
    public class HomeController : Controller
    {
        //
        // GET: /Admin/Home/

        public ActionResult Index()
        {
            return RedirectToAction("Index", "General");
        }

        public ActionResult Random()
        {
            return Content(Convert.ToBase64String(CryptoRandom.CreateRandomKey(32)), "text/plain");
        }

    }
}
