//using Microsoft.Owin.Security;
//using Microsoft.Owin.Security.Infrastructure;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Security.Principal;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;

//namespace Thinktecture.IdentityModel.Oidc.OWIN
//{
//    public class OpenIdConnectAuthenticationHandler : AuthenticationHandler<OpenIdConnectAuthenticationOptions>
//    {
//        protected override async Task<AuthenticationTicket> AuthenticateCore()
//        {
//            var query = HttpUtility.ParseQueryString(Request.QueryString);
//            var response = OidcClient.HandleAuthorizeResponse(query);

//            if (string.IsNullOrWhiteSpace(response.Code))
//            {
//                return null;
//            }

//            if (response.IsError)
//            {
//                throw new Exception("authZ response error: " + response.Error);
//            }

//            return new AuthenticationTicket(
//                new GenericIdentity("dominick", "OIDC"),
//                new Dictionary<string, string>(StringComparer.Ordinal));
//        }

//        // if returns true - pipeline will end
//        public override async Task<bool> Invoke()
//        {
//            return await InvokeReplyPath();
//        }

//        private async Task<bool> InvokeReplyPath()
//        {
//            if (Options.ReturnPath != null &&
//                String.Equals(Options.ReturnPath, Request.Path, StringComparison.OrdinalIgnoreCase))
//            {
//                var model = await Authenticate();

//                if (model == null)
//                {
//                    return false;
//                }
                
//                string redirectUri = model.Extra.RedirectUrl;

//                if (!string.IsNullOrEmpty(Options.SigninAsAuthenticationType))
//                {
//                    ClaimsIdentity grantIdentity = model.Identity;
//                    if (!string.Equals(grantIdentity.AuthenticationType, Options.SigninAsAuthenticationType, StringComparison.Ordinal))
//                    {
//                        grantIdentity = new ClaimsIdentity(grantIdentity.Claims, Options.SigninAsAuthenticationType, grantIdentity.NameClaimType, grantIdentity.RoleClaimType);
//                    }

//                    Response.Grant(grantIdentity, model.Extra);
//                }

//                Response.Redirect("https://localhost:44310/");
//                return true;
//            }
//            return false;
//        }

//        protected override async Task ApplyResponseChallenge()
//        {
//            if (Response.StatusCode != 401)
//            {
//                return;
//            }

//            //var challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);

//            var url = OidcClient.GetAuthorizeUrl(
//                Options.AuthorizeEndpoint,
//                Options.RedirectUri,
//                Options.ClientId,
//                Options.Scope,
//                "abc");

//            Response.Redirect(url.AbsoluteUri);
//        }
//    }
//}
