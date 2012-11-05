using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Protocols.OAuth2
{
    class OAuthTitleGrantResult : ActionResult
    {
        public string TokenString { get; set; }
        public string RedirectUrl { get; set; }
        
        public OAuthTitleGrantResult(string tokenString, string redirectUrl)
        {
            this.TokenString = tokenString;
            this.RedirectUrl = redirectUrl;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.RequestContext.HttpContext.Response;

            var html = String.Format(
                "<html><head><title>{0}</title></head><body>Authorized</body></html>",
                this.TokenString);

            response.Clear();
            response.StatusCode = 302;
            response.ContentType = "text/html";
            response.Headers.Add("Location", this.RedirectUrl);
            response.Write(html);
            response.End();
        }
    }
}
