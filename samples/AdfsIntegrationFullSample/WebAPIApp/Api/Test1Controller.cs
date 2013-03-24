using MsftJwtIdentityModelExtensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebAPIApp.Api
{
    public class Test1Controller : ApiController
    {
        public async Task<HttpResponseMessage> Get(bool callTest2=false)
        {
            var claims =
                from c in ClaimsPrincipal.Current.Claims
                select new
                {
                    c.Type, c.Value, c.Issuer, c.OriginalIssuer
                };

            var test2Claims = await CallTest2(callTest2);

            var result = new
            {
                ClaimsForTest1 = claims.ToArray(),
                ClaimsFromTest2 = test2Claims
            };

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        private async Task<object> CallTest2(bool callTest2)
        {
            if (callTest2 == false) return null;

            var adfsIntegrationUrl = "https://idsrv.local/issue/adfs";
            var webAPIId = "http://localhost/rp-adfs-webapi2";
            var webAPIService = "https://localhost/rp-adfs-webapi/api/test2";

            // call adfs integration to convert saml to jwt for webapi RP
            var adfsProxy = new AdfsIntegrationProxy(adfsIntegrationUrl);
            var token = Request.Headers.Authorization.Parameter;
            var jwt = await adfsProxy.JwtToJwtAsync(token, webAPIId);

            // call webapi RP with jwt
            var client = new HttpClient { BaseAddress = new Uri(webAPIService) };
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

            var response = await client.GetAsync("");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JArray.Parse(json);
        }
    }
}
