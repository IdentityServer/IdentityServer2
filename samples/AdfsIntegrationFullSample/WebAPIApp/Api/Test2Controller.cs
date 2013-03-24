using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace WebAPIApp.Api
{
    public class Test2Controller : ApiController
    {
        public HttpResponseMessage Get()
        {
            var claims =
                from c in ClaimsPrincipal.Current.Claims
                select new
                {
                    c.Type, c.Value, c.Issuer, c.OriginalIssuer
                };
            return Request.CreateResponse(HttpStatusCode.OK, claims.ToArray());
        }
    }
}
