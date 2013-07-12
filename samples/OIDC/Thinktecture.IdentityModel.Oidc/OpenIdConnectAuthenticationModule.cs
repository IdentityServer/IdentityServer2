using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Thinktecture.IdentityModel.Oidc
{
    public class OpenIdConnectAuthenticationModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += OnAuthenticateRequestRequest;
            context.EndRequest += OnEndRequest;
        }

        void OnEndRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;

            // check for 401 - turn to 302 to oidc provider

            if (context.Response.StatusCode == 401 && context.User.Identity.IsAuthenticated)
            {
                context.Response.Redirect("https://idsrv.local/issue/oidc/authorize?client_id=oidccode");
            }
        }

        void OnAuthenticateRequestRequest(object sender, EventArgs e)
        {
            // check for callback and do back channel communication
            // establish session afterwards
        }

        public void Dispose()
        { }

    }
}
