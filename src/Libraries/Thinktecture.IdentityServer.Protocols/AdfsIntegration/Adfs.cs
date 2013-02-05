using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Thinktecture.IdentityServer.Protocols.OAuth2;

namespace Thinktecture.IdentityServer.Protocols.AdfsIntegration
{
    public class Adfs : ApiController
    {
        public HttpResponseMessage Post(TokenRequest request)
        {
            throw new NotImplementedException("Adfs.Post");
        }
    }
}
