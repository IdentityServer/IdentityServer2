using MsftJwtIdentityModelExtensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebRP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.HttpMethod == "POST")
            {
                return RedirectToAction("Index");
            }


            var bootstrapCtx = ClaimsPrincipal.Current.Identities.First().BootstrapContext as BootstrapContext;
            if (bootstrapCtx != null)
            {
                var securityToken = bootstrapCtx.SecurityToken;
                var token = bootstrapCtx.Token;
            }
            
            
            return View();
        }

        public ActionResult Login()
        {
            var url = FederatedAuthentication.WSFederationAuthenticationModule.CreateSignInRequest(null, null, false).RequestUrl; ;
            return Redirect(url);
        }
        
        public ActionResult Logout()
        {
            FederatedAuthentication.WSFederationAuthenticationModule.SignOut();
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<ActionResult> CallWebAPI1()
        {
            var json = await CallWebAPI(false);
            return Content(json, "application/json");
        }

        [Authorize]
        public async Task<ActionResult> CallWebAPI2()
        {
            var json = await CallWebAPI(true);
            return Content(json, "application/json");
        }

        async Task<string> CallWebAPI(bool callTest2)
        {
            var adfsIntegrationUrl = "https://idsrv.local/issue/adfs";
            var webAPIId = "http://localhost/rp-adfs-webapi1";
            var webAPIService = "https://localhost/rp-adfs-webapi/api/test1";

            // call adfs integration to convert saml to jwt for webapi RP
            var adfsProxy = new AdfsIntegrationProxy(adfsIntegrationUrl);
            string jwt = null;
            // need original token to get new token
            var bootstrapCtx = ClaimsPrincipal.Current.Identities.First().BootstrapContext as BootstrapContext;
            if (bootstrapCtx.SecurityToken != null)
            {
                jwt = await adfsProxy.SamlToJwtAsync(bootstrapCtx.SecurityToken, webAPIId);
            }
            else if (bootstrapCtx.Token != null)
            {
                jwt = await adfsProxy.SamlToJwtAsync(bootstrapCtx.Token, webAPIId);
            }
            else
            {
                throw new Exception("No bootstrap context token available");
            }

            // call webapi RP with jwt
            var client = new HttpClient { BaseAddress = new Uri(webAPIService) };
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

            var response = await client.GetAsync("?callTest2=" + callTest2.ToString());
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return json;
        }
    }

}
