using System.Web.Http;

namespace OwinDemo
{
    public class IdentityController : ApiController
    {
        [Authorize]
        public string Get()
        {
            return User.Identity.Name;
        }
    }
}