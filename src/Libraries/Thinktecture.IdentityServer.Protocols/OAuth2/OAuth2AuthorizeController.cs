using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Protocols.OAuth2
{
    class OAuth2AuthorizeController : Controller
    {
        [ActionName("Index")]
        [HttpGet]
        public ActionResult HandleRequest()
        {
            return null;
        }

        [ActionName("Index")]
        [HttpPost]
        public ActionResult HandleResponse()
        {
            return null;
        }
    }
}
