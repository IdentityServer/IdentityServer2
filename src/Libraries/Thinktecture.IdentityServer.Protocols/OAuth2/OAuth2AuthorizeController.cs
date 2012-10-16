using System;
using System.ComponentModel.Composition;
using System.IdentityModel.Protocols.WSTrust;
using System.Security.Claims;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Authorization.Mvc;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Protocols.OAuth2
{
    [ClaimsAuthorize(Constants.Actions.Issue, Constants.Resources.OAuth2)]
    public class OAuth2AuthorizeController : Controller
    {
        [Import]
        public IClientsRepository Clients { get; set; }

        [Import]
        public IConfigurationRepository Configuration { get; set; }

        public OAuth2AuthorizeController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public OAuth2AuthorizeController(IConfigurationRepository configuration, IClientsRepository client)
        {
            Configuration = configuration;
            Clients = client;
        }

        [ActionName("Index")]
        [HttpGet]
        public ActionResult HandleRequest(AuthorizeRequest request)
        {
            // check required fields
            if (!ModelState.IsValid)
            {
                // return the right error
                throw new Exception("Invalid model");
            }

            // validate client
            Client client;
            if (!Clients.TryGetClient(request.client_id, out client))
            {
                // return the right error
                throw new Exception("invalid client.");
            }

            // validate scope (must be a valid URI)
            Uri uri;
            if (!Uri.TryCreate(request.scope, UriKind.Absolute, out uri))
            {
                // return the right error
                throw new Exception("invalid scope.");
            }

            // implicit grant
            if (request.response_type.Equals(OAuth2Constants.ResponseTypes.Token, StringComparison.Ordinal))
            {
                return HandleImplicitGrant(request, client);
            }

            // authorization code grant
            if (request.response_type.Equals(OAuth2Constants.ResponseTypes.Code, StringComparison.Ordinal))
            {
                return HandleAuthorizationCodeGrant(request, client);
            }

            // todo: return appropiate error
            throw new Exception("invalid response type.");
        }

        [ActionName("Index")]
        [HttpPost]
        public ActionResult HandleResponse()
        {
            return null;
        }


        private ActionResult HandleAuthorizationCodeGrant(AuthorizeRequest request, Client client)
        {
            throw new System.NotImplementedException();
        }

        private ActionResult HandleImplicitGrant(AuthorizeRequest request, Client client)
        {
            var sts = new STS();
            
            TokenResponse tokenResponse;
            if (sts.TryIssueToken(
                    new EndpointReference(request.scope),
                    ClaimsPrincipal.Current,
                    Configuration.Global.DefaultHttpTokenType,
                    out tokenResponse))
            {
                var redirectString = string.Format("{0}#access_token={1}&token_type={2}&expires_in={3}",
                        client.RedirectUri.AbsoluteUri,
                        tokenResponse.AccessToken,
                        tokenResponse.TokenType,
                        tokenResponse.ExpiresIn);

                if (!string.IsNullOrEmpty(request.state))
                {
                    redirectString = string.Format("{0}&state={1}", redirectString, request.state);
                }

                return Redirect(redirectString);
            }

            // return right error code
            throw new Exception("token issuance failed.");
        }

    }
}
