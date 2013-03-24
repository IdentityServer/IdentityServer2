using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebRP.Api
{
    public class HomeController : ApiController
    {
        [Authorize]
        public HttpResponseMessage Post()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "WebApi!");
        }
    }
}
