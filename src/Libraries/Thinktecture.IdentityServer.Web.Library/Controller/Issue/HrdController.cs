/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.Composition;
using System.IdentityModel.Services;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.TokenService;
using Thinktecture.IdentityServer.Web.ActionResults;
using Thinktecture.IdentityServer.Web.Security;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    public class HrdController : Controller
    {
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

            if (!ConfigurationRepository.Endpoints.WSFederation)
            {
                return new HttpNotFoundResult();
            }

            var message = WSFederationMessage.CreateFromUri(HttpContext.Request.Url);

            // sign in 
            var signinMessage = message as SignInRequestMessage;
            if (signinMessage != null)
            {
                return ProcessSignInRequest(signinMessage);
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
                return ShowHomeRealmSelection();
            }

            //// issue token and create ws-fed response
            //var response = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(
            //    message,
            //    principal as ClaimsPrincipal,
            //    TokenServiceConfiguration.Current.CreateSecurityTokenService());

            //// set cookie for single-sign-out
            //new SignInSessionsManager(HttpContext, ConfigurationRepository.Configuration.MaximumTokenLifetime)
            //    .AddRealm(response.BaseUri.AbsoluteUri);

            //return new WSFederationResult(response);
        }

        private ActionResult ShowHomeRealmSelection()
        {
            throw new System.NotImplementedException();
        }

        private ActionResult RedirectToIdentityProvider(SignInRequestMessage message)
        {
            IdentityProvider idp = null;
            if (IdentityProviderRepository.TryGet(message.HomeRealm, out idp))
            {
                return RedirectToIdentityProvider(idp, message);
            }

            return View("Error");
        }

        private ActionResult RedirectToIdentityProvider(IdentityProvider identityProvider, SignInRequestMessage request)
        {
            var message = new SignInRequestMessage(new Uri(identityProvider.WSFederationEndpoint), request.Realm);

            // TODO: handle context differently
            message.Context = request.Context;

            return new RedirectResult(message.WriteQueryString());
        }

       
        #endregion
    }
}
