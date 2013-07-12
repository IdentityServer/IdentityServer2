using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Configuration;
using Thinktecture.IdentityModel.Web;

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
            
            if (context.Response.StatusCode == 401 && 
                !context.User.Identity.IsAuthenticated &&
                !context.Response.SuppressFormsAuthenticationRedirect)
            {
                var state = Guid.NewGuid().ToString("N");
                var returnUrl = HttpUtility.UrlEncode(context.Request.RawUrl, context.Request.ContentEncoding);
                state = state + "_" + returnUrl;
                
                var authorizeUrl = WebConfigurationManager.AppSettings["oidc:authorizeUrl"];
                var clientId = WebConfigurationManager.AppSettings["oidc:clientId"];
                var scopes = "openid " + WebConfigurationManager.AppSettings["oidc:scopes"];
                var redirectUri = "https://localhost:44309/" + "oidccallback";
                
                var queryString = string.Format("?client_id={0}&scope={1}&redirect_uri={2}&state={3}&response_type=code",
                    clientId,
                    scopes,
                    redirectUri,
                    state);

                var cookie = new ProtectedCookie(ProtectionMode.MachineKey);
                cookie.Write("oidcstate", state, DateTime.UtcNow.AddHours(1));
                
                context.Response.Redirect("https://idsrv.local/issue/oidc/authorize" + queryString);
            }
        }

        void OnAuthenticateRequestRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;

            if (context.Request.Url.AbsoluteUri.Equals("https://localhost:44309/" + "oidccallback", StringComparison.OrdinalIgnoreCase))
            {
                var code = context.Request.QueryString["code"];
                var state = context.Request.QueryString["state"];
                var error = context.Request.QueryString["error"];

                var tokenUrl = WebConfigurationManager.AppSettings["oidc:tokenUrl"];
                var clientId = WebConfigurationManager.AppSettings["oidc:clientId"];
                var clientSecret = WebConfigurationManager.AppSettings["oidc:clientSecret"];
                
                var cookie = new ProtectedCookie(ProtectionMode.MachineKey);
                var storedState = cookie.Read("oidcstate");

                if (storedState != state)
                {
                    throw new InvalidOperationException("state invalid.");
                }

                var returnUrl = state.Split('_')[1];

                // do back channel communication
                //  token endpoint
                //  userinfo endpoint
                var client = new HttpClient
                {
                    BaseAddress = new Uri(tokenUrl)
                };
                
                client.SetBasicAuthentication(clientId, clientSecret);

                var parameter = new Dictionary<string, string>
                {
                    { "grant_type", "authorization_code" },
                    { "code", code },
                    { "redirect_uri", "https://localhost:44309/" + "oidccallback" }
                };
                
                var response = client.PostAsync("", new FormUrlEncodedContent(parameter)).Result;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new InvalidOperationException("error calling token endpoint");
                }

                var oidcResponse = response.Content.ReadAsStringAsync().Result;
                var oidcJson = JObject.Parse(oidcResponse);

                var idToken = oidcJson["id_token"].ToString();
                var principal = ValidateIdentityToken(idToken);

                // establish session
                var sessionToken = new SessionSecurityToken(principal);
                FederatedAuthentication.SessionAuthenticationModule.WriteSessionTokenToCookie(sessionToken);

                
                // redirect local to return url
                context.Response.Redirect(returnUrl);
            }

            
        }

        private ClaimsPrincipal ValidateIdentityToken(string idToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        { }

    }
}
