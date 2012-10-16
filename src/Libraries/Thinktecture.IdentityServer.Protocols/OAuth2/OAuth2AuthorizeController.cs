using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Protocols.OAuth2
{
    public class OAuth2AuthorizeController : Controller
    {
        [ActionName("Index")]
        [HttpGet]
        public ActionResult HandleRequest(AuthorizeRequest request)
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

    public class AuthorizeRequest
    {
        public string response_type { get; set; }
        public string client_id { get; set; }
        public string redirect_uri { get; set; }
        public string scope { get; set; }
        public string state { get; set; }
    }
}
