/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.Composition;
using System.IdentityModel.Selectors;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.TokenService;
using Thinktecture.IdentityModel.Web;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Thinktecture.IdentityServer.Protocols.WSFederation
{
    public class HrdController : Controller
    {
        const string _cookieName = "hrdsignout";
        const string _cookieNameRememberHrd = "hrdSelection";

        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        [Import]
        public IIdentityProviderRepository IdentityProviderRepository { get; set; }


        public HrdController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public HrdController(IConfigurationRepository configurationRepository, IIdentityProviderRepository identityProviderRepository)
        {
            IdentityProviderRepository = identityProviderRepository;
            ConfigurationRepository = configurationRepository;
        }

        public ActionResult Issue()
        {
            Tracing.Verbose("HRD endpoint called.");

            var message = WSFederationMessage.CreateFromUri(HttpContext.Request.Url);

            // sign in 
            var signinMessage = message as SignInRequestMessage;
            if (signinMessage != null)
            {
                return ProcessSignInRequest(signinMessage);
            }

            // sign out
            var signoutMessage = message as SignOutRequestMessage;
            if (signoutMessage != null)
            {
                return ProcessSignOut(signoutMessage);
            }

            return View("Error");
        }

        [HttpPost]
        [ActionName("Issue")]
        public ActionResult IssueResponse()
        {
            var fam = new WSFederationAuthenticationModule();
            fam.FederationConfiguration = new FederationConfiguration();

            if (fam.CanReadSignInResponse(Request))
            {
                var responseMessage = fam.GetSignInResponseMessage(Request);
                return ProcessSignInResponse(responseMessage, fam.GetSecurityToken(Request));
            }

            return View("Error");
        }

        #region Helper
        private ActionResult ProcessSignInRequest(SignInRequestMessage message)
        {
            if (!string.IsNullOrWhiteSpace(message.HomeRealm))
            {
                return RedirectToIdentityProvider(message);
            }
            else
            {
                var pastHRDSelection = GetRememberHRDCookieValue();
                if (String.IsNullOrWhiteSpace(pastHRDSelection))
                {
                    return ShowHomeRealmSelection(message);
                }
                else
                {
                    return ProcessHomeRealmFromCookieValue(message, pastHRDSelection);
                }
            }
        }

        private ActionResult ProcessHomeRealmFromCookieValue(SignInRequestMessage message, string pastHRDSelection)
        {
            message.HomeRealm = pastHRDSelection;
            return ProcessSignInRequest(message);
        }

        private ActionResult ProcessSignOut(SignOutRequestMessage message)
        {
            // check for return url
            if (!string.IsNullOrWhiteSpace(message.Reply))
            {
                ViewBag.ReturnUrl = message.Reply;
            }

            // check for existing sign in sessions
            var mgr = new SignInSessionsManager(HttpContext, _cookieName);
            var realms = mgr.GetEndpoints();
            mgr.ClearEndpoints();

            return View("Signout", realms);
        }

        private ActionResult ProcessSignInResponse(SignInResponseMessage responseMessage, SecurityToken token)
        {
            var principal = ValidateToken(token);
            var issuerName = principal.Claims.First().Issuer;

            principal.Identities.First().AddClaim(
                new Claim(Constants.Claims.IdentityProvider, issuerName, ClaimValueTypes.String, Constants.InternalIssuer));

            var context = GetContextCookie();
            var message = new SignInRequestMessage(new Uri("http://foo"), context.Realm);
            message.Context = context.Wctx;

            // issue token and create ws-fed response
            var wsFedResponse = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(
                message,
                principal,
                TokenServiceConfiguration.Current.CreateSecurityTokenService());

            // set cookie for single-sign-out
            new SignInSessionsManager(HttpContext, _cookieName, ConfigurationRepository.Global.MaximumTokenLifetime)
                .SetEndpoint(context.WsFedEndpoint);

            return new WSFederationResult(wsFedResponse, requireSsl: ConfigurationRepository.WSFederation.RequireSslForReplyTo);
        }

        IEnumerable<IdentityProvider> GetEnabledWSIdentityProviders()
        {
            return IdentityProviderRepository.GetAll().Where(
                x => x.Enabled && x.Type == IdentityProviderTypes.WSStar);
        }
        IEnumerable<IdentityProvider> GetVisibleIdentityProviders()
        {
            return IdentityProviderRepository.GetAll().Where(
                x => x.Enabled && x.ShowInHrdSelection);
        }

        private ClaimsPrincipal ValidateToken(SecurityToken token)
        {
            var config = new SecurityTokenHandlerConfiguration();
            config.AudienceRestriction.AudienceMode = AudienceUriMode.Always;
            config.AudienceRestriction.AllowedAudienceUris.Add(new Uri(ConfigurationRepository.Global.IssuerUri));

            var registry = new IdentityProviderIssuerNameRegistry(GetEnabledWSIdentityProviders());
            config.IssuerNameRegistry = registry;
            config.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
            config.CertificateValidator = X509CertificateValidator.None;

            var handler = SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection(config);
            var identity = handler.ValidateToken(token).First();

            return new ClaimsPrincipal(identity);
        }


        private ActionResult ShowHomeRealmSelection(SignInRequestMessage message)
        {
            var idps = GetVisibleIdentityProviders();
            if (idps.Count() == 1)
            {
                var ip = idps.First();
                message.HomeRealm = ip.Name;
                Tracing.Verbose("Only one HRD option available: " + message.HomeRealm);
                return RedirectToIdentityProvider(ip, message);
            }
            else
            {
                Tracing.Verbose("HRD selection screen displayed.");
                var vm = new HrdViewModel(message, idps);
                return View("HRD", vm);
            }
        }

        private string GetRememberHRDCookieValue()
        {
            if (Request.Cookies.AllKeys.Contains(_cookieNameRememberHrd))
            {
                var cookie = Request.Cookies[_cookieNameRememberHrd];
                var realm = cookie.Value;
                var idps = GetVisibleIdentityProviders().Where(x => x.Name == realm);
                var idp = idps.SingleOrDefault();
                if (idp == null)
                {
                    Tracing.Verbose("Past HRD selection from cookie not found in current HRD list. Past value was: " + realm);
                    SetRememberHRDCookieValue(null);
                }
                return realm;
            }
            return null;
        }

        private void SetRememberHRDCookieValue(string realm)
        {
            if (String.IsNullOrWhiteSpace(realm))
            {
                Response.Cookies.Remove(_cookieNameRememberHrd);
                return;
            }

            var cookie = new HttpCookie(_cookieNameRememberHrd, realm);
            cookie.Expires = DateTime.Now.AddMonths(1);
            cookie.HttpOnly = true;
            cookie.Secure = true;
            cookie.Path = Request.ApplicationPath;
            Response.Cookies.Add(cookie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Select")]
        public ActionResult ProcessHRDSelection(string idp, string originalSigninUrl, bool rememberHRDSelection = false)
        {
            Tracing.Verbose("HRD selected: " + idp);

            var uri = new Uri(originalSigninUrl);
            var message = WSFederationMessage.CreateFromUri(uri);
            var signinMessage = message as SignInRequestMessage;

            var ip = GetVisibleIdentityProviders().Where(x => x.Name == idp).FirstOrDefault();
            if (ip == null || signinMessage == null) return View("Error");

            try
            {
                if (rememberHRDSelection)
                {
                    SetRememberHRDCookieValue(idp);
                }

                if (ip.Type == IdentityProviderTypes.WSStar)
                {
                    signinMessage.HomeRealm = ip.Name;
                    return RedirectToIdentityProvider(ip, signinMessage);
                }

                if (ip.Type == IdentityProviderTypes.OAuth2)
                {
                    return ProcessOAuthSignIn(ip, signinMessage);
                }
            }
            catch (Exception ex)
            {
                Tracing.Error(ex.ToString());
            }

            return View("Error");
        }

        private ActionResult RedirectToIdentityProvider(SignInRequestMessage request)
        {
            IdentityProvider idp = null;
            if (IdentityProviderRepository.TryGet(request.HomeRealm, out idp) && idp.Enabled)
            {
                return RedirectToIdentityProvider(idp, request);
            }

            return View("Error");
        }

        private ActionResult RedirectToIdentityProvider(IdentityProvider identityProvider, SignInRequestMessage request)
        {
            var message = new SignInRequestMessage(new Uri(identityProvider.WSFederationEndpoint), ConfigurationRepository.Global.IssuerUri);
            SetContextCookie(request.Context, request.Realm, identityProvider.WSFederationEndpoint);

            return new RedirectResult(message.WriteQueryString());
        }

        private void SetContextCookie(string wctx, string realm, string wsfedEndpoint)
        {
            var j = JObject.FromObject(new Context { Wctx = wctx, Realm = realm, WsFedEndpoint = wsfedEndpoint });

            var cookie = new HttpCookie("idsrvcontext", j.ToString());
            cookie.Secure = true;
            cookie.HttpOnly = true;

            Response.Cookies.Add(cookie);
        }

        private Context GetContextCookie()
        {
            var cookie = Request.Cookies["idsrvcontext"];
            if (cookie == null)
            {
                throw new InvalidOperationException("cookie");
            }

            var json = JObject.Parse(HttpUtility.UrlDecode(cookie.Value));
            return json.ToObject<Context>();
        }

        internal class Context
        {
            public string Wctx { get; set; }
            public string Realm { get; set; }
            public string WsFedEndpoint { get; set; }
        }

        internal class OAuthContext : Context
        {
            public int IdP { get; set; }
        }

        private void SetOAuthContextCookie(OAuthContext ctx)
        {
            var j = JObject.FromObject(ctx);

            var cookie = new HttpCookie("idsrvoauthcontext", j.ToString());
            cookie.Secure = true;
            cookie.HttpOnly = true;
            cookie.Path = Request.ApplicationPath;

            Response.Cookies.Add(cookie);
        }

        private OAuthContext GetOAuthContextCookie()
        {
            var cookie = Request.Cookies["idsrvoauthcontext"];
            if (cookie == null)
            {
                throw new InvalidOperationException("cookie");
            }

            var json = JObject.Parse(HttpUtility.UrlDecode(cookie.Value));
            return json.ToObject<OAuthContext>();
        }

        private ActionResult ProcessOAuthSignIn(IdentityProvider ip, SignInRequestMessage request)
        {
            switch (ip.ProfileType)
            {
                case OAuthProfileTypes.Google:
                    return ProcessOAuthSignInGoogle(ip, request);
            }

            return View("Error");
        }

        private ActionResult ProcessOAuthSignInGoogle(IdentityProvider ip, SignInRequestMessage request)
        {
            var scope = "https://www.googleapis.com/auth/userinfo.profile";
            var state = Thinktecture.IdentityModel.CryptoRandom.CreateRandomKeyString(10);
            var redirectUri = String.Format("https://{0}{1}",
                Request.Url.Host,
                Url.Action("OAuthTokenCallback"));
            var url = String.Format("{0}?scope={1}&state={2}&redirect_uri={3}&response_type=code&client_id={4}",
                ip.AuthorizationUrl,
                scope,
                state,
                redirectUri,
                ip.ClientID);
            var ctx = new OAuthContext
            {
                Wctx = request.Context,
                Realm = request.Realm,
                IdP = ip.ID
            };
            SetOAuthContextCookie(ctx);
            return Redirect(url);
        }

        [HttpGet]
        public ActionResult OAuthTokenCallback(string code, string state, string error)
        {
            if (!String.IsNullOrWhiteSpace(code))
            {
                var ctx = GetOAuthContextCookie();
                if (ctx != null)
                {
                    var ip = GetVisibleIdentityProviders().Where(x => x.ID == ctx.IdP).FirstOrDefault();
                    if (ip != null && ip.Type == IdentityProviderTypes.OAuth2)
                    {
                        var p = GetOAuthClaimsGoogle(code, ip);
                        return ProcessOAuthResponse(p, ctx);
                    }
                }
            }
            return View("Error");
        }

        string GetOAuthTokenGoogle(string code, IdentityProvider ip)
        {
            HttpClient client = new HttpClient();
            var redirectUri = String.Format("https://{0}{1}",
                Request.Url.Host,
                Url.Action("OAuthTokenCallback"));
            List<KeyValuePair<string, string>> postValues =
                new List<KeyValuePair<string, string>>();
            postValues.Add(new KeyValuePair<string, string>("code", code));
            postValues.Add(new KeyValuePair<string, string>("client_id", ip.ClientID));
            postValues.Add(new KeyValuePair<string, string>("client_secret", ip.ClientSecret));
            postValues.Add(new KeyValuePair<string, string>("redirect_uri", redirectUri));
            postValues.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            var content = new FormUrlEncodedContent(postValues);
            var result = client.PostAsync("https://accounts.google.com/o/oauth2/token", content).Result;
            if (result.IsSuccessStatusCode)
            {
                var response = result.Content.ReadAsAsync<TokenResponse>().Result;
                return response.AccessToken;
            }

            return null;
        }
        
        IEnumerable<Claim> GetOAuthProfileGoogle(string accessKey, IdentityProvider ip)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessKey);
            var result = client.GetAsync("https://www.googleapis.com/oauth2/v1/userinfo").Result;
            if (result.IsSuccessStatusCode)
            {
                var response = result.Content.ReadAsStringAsync().Result;
                var profile = JObject.Parse(response);
                
                var issuer = ip.Name;
                var claims = new List<Claim>();
                claims.Add(new Claim(Constants.Claims.IdentityProvider, issuer, ClaimValueTypes.String, Constants.InternalIssuer));
                
                claims.Add(new Claim(ClaimTypes.NameIdentifier, profile.Value<string>("id"), ClaimValueTypes.String, issuer));
                claims.Add(new Claim(ClaimTypes.Email, profile.Value<string>("email"), ClaimValueTypes.String, issuer));
                claims.Add(new Claim(ClaimTypes.Name, profile.Value<string>("name"), ClaimValueTypes.String, issuer));
                claims.Add(new Claim(ClaimTypes.GivenName, profile.Value<string>("given_name"), ClaimValueTypes.String, issuer));
                claims.Add(new Claim(ClaimTypes.Surname, profile.Value<string>("family_name"), ClaimValueTypes.String, issuer));
                claims.Add(new Claim(ClaimTypes.Gender, profile.Value<string>("gender"), ClaimValueTypes.String, issuer));
                return claims;
            }

            return null;
        }

        private ClaimsPrincipal GetOAuthClaimsGoogle(string code, IdentityProvider ip)
        {
            var token = GetOAuthTokenGoogle(code, ip);
            if (token != null)
            {
                var claims = GetOAuthProfileGoogle(token, ip);
                var id = new ClaimsIdentity(claims, "OAuth");
                return new ClaimsPrincipal(id);
            }
            return null;
        }

        private ActionResult ProcessOAuthResponse(ClaimsPrincipal principal, Context context)
        {
            var message = new SignInRequestMessage(new Uri("http://foo"), context.Realm);
            message.Context = context.Wctx;

            // issue token and create ws-fed response
            var wsFedResponse = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(
                message,
                principal,
                TokenServiceConfiguration.Current.CreateSecurityTokenService());

            // set cookie for single-sign-out
            new SignInSessionsManager(HttpContext, _cookieName, ConfigurationRepository.Global.MaximumTokenLifetime)
                .SetEndpoint(context.WsFedEndpoint);

            return new WSFederationResult(wsFedResponse, requireSsl: ConfigurationRepository.WSFederation.RequireSslForReplyTo);
        }

        #endregion
    }
}
