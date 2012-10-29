/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using BrockAllen.OAuth2;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IdentityModel.Selectors;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.TokenService;

namespace Thinktecture.IdentityServer.Protocols.WSFederation
{
    public class HrdController : Controller
    {
        const string _cookieName = "hrdsignout";
        const string _cookieNameRememberHrd = "hrdSelection";
        const string _cookieContext = "idsrvcontext";

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
            var cookie = new HttpCookie(_cookieNameRememberHrd);
            if (String.IsNullOrWhiteSpace(realm))
            {
                realm = ".";
                cookie.Expires = DateTime.UtcNow.AddYears(-1);
            }
            else
            {
                cookie.Expires = DateTime.Now.AddMonths(1);
            }
            cookie.Value = realm;
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
                    return ProcessOAuth2SignIn(ip, signinMessage);
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

            var cookie = new HttpCookie(_cookieContext, j.ToString())
            {
                Secure = true,
                HttpOnly = true,
                Path = HttpRuntime.AppDomainAppVirtualPath
            };
            
            Response.Cookies.Add(cookie);
        }

        private Context GetContextCookie()
        {
            var cookie = Request.Cookies[_cookieContext];
            if (cookie == null)
            {
                throw new InvalidOperationException("cookie");
            }

            var json = JObject.Parse(HttpUtility.UrlDecode(cookie.Value));

            cookie.Value = "";
            cookie.Expires = new DateTime(2000, 1, 1);
            cookie.Path = HttpRuntime.AppDomainAppVirtualPath;
            Response.SetCookie(cookie);

            return json.ToObject<Context>();
        }

        internal class Context
        {
            public string Wctx { get; set; }
            public string Realm { get; set; }
            public string WsFedEndpoint { get; set; }
        }

        internal class OAuth2Context : Context
        {
            public int IdP { get; set; }
        }

        private void SetOAuthContextCookie(OAuth2Context ctx)
        {
            var j = JObject.FromObject(ctx);

            var cookie = new HttpCookie("idsrvoauthcontext", j.ToString());
            cookie.Secure = true;
            cookie.HttpOnly = true;
            cookie.Path = Request.ApplicationPath;

            Response.Cookies.Add(cookie);
        }

        private OAuth2Context GetOAuthContextCookie()
        {
            var cookie = Request.Cookies["idsrvoauthcontext"];
            if (cookie == null)
            {
                throw new InvalidOperationException("cookie");
            }

            var json = JObject.Parse(HttpUtility.UrlDecode(cookie.Value));
            return json.ToObject<OAuth2Context>();
        }

        private ActionResult ProcessOAuth2SignIn(IdentityProvider ip, SignInRequestMessage request)
        {
            var ctx = new OAuth2Context
            {
                Wctx = request.Context,
                Realm = request.Realm,
                IdP = ip.ID
            };
            SetOAuthContextCookie(ctx);

            var oauth2 = new OAuth2Client(GetProviderTypeFromOAuthProfileTypes(ip.ProviderType.Value), ip.ClientID, ip.ClientSecret);
            switch (ip.ProviderType)
            {
                case OAuth2ProviderTypes.Google:
                    return new OAuth2ActionResult(oauth2, ProviderType.Google, null);
                case OAuth2ProviderTypes.Facebook:
                    return new OAuth2ActionResult(oauth2, ProviderType.Facebook, null);
                case OAuth2ProviderTypes.Live:
                    return new OAuth2ActionResult(oauth2, ProviderType.Live, null);
            }

            return View("Error");
        }

        ProviderType GetProviderTypeFromOAuthProfileTypes(OAuth2ProviderTypes type)
        {
            switch (type)
            {
                case OAuth2ProviderTypes.Facebook: return ProviderType.Facebook;
                case OAuth2ProviderTypes.Live: return ProviderType.Live;
                case OAuth2ProviderTypes.Google: return ProviderType.Google;
                default: throw new Exception("Invalid OAuthProfileTypes");
            }
        }

        [HttpGet]
        public async Task<ActionResult> OAuthTokenCallback()
        {
            var ctx = GetOAuthContextCookie();
            var ip = GetVisibleIdentityProviders().Single(x => x.ID == ctx.IdP);

            var oauth2 = new OAuth2Client(GetProviderTypeFromOAuthProfileTypes(ip.ProviderType.Value), ip.ClientID, ip.ClientSecret);
            var result = await oauth2.ProcessCallbackAsync();
            if (result.Error != null) return View("Error");
            
            var claims = result.Claims.ToList();
            string[] claimsToRemove = new string[]
            {
                "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                ClaimTypes.AuthenticationInstant
            };
            foreach(var toRemove in claimsToRemove)
            {
                var tmp = claims.Find(x=>x.Type == toRemove);
                if (tmp != null) claims.Remove(tmp);
            }
            claims.Add(new Claim(Constants.Claims.IdentityProvider, ip.Name, ClaimValueTypes.String, Constants.InternalIssuer));
            var id = new ClaimsIdentity(claims, "OAuth");
            var cp = new ClaimsPrincipal(id);
            return ProcessOAuthResponse(cp, ctx);
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
