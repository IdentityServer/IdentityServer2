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
            //
            // first round of validation:
            // missing, invalid, or mismatching redirection URI or
            // missing or invalid client id
            // show error page to user
            //

            // validate client
            Client client;
            if (!Clients.TryGetClient(request.client_id, out client))
            {
                // todo: show error page to user
                throw new Exception("invalid client.");
            }

            // validate redirect uri
            if (string.IsNullOrEmpty(request.redirect_uri) || !string.Equals(request.redirect_uri, client.RedirectUri.AbsoluteUri, StringComparison.OrdinalIgnoreCase))
            {
                // todo: show error page to user
                throw new Exception("invalid client.");
            }

            // validate scope (must be a valid URI)
            Uri uri;
            if (!Uri.TryCreate(request.scope, UriKind.Absolute, out uri))
            {
                return Error(client.RedirectUri, OAuth2Constants.Errors.InvalidScope, request.state);
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
            return Error(client.RedirectUri, OAuth2Constants.Errors.UnsupportedResponseType, request.state);
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
            return Error(client.RedirectUri, OAuth2Constants.Errors.InvalidRequest, request.state);
        }

        private ActionResult Error(Uri redirectUri, string error, string state = null)
        {
            string url;

            if (string.IsNullOrEmpty(state))
            {
                url = string.Format("{0}#error={1}", redirectUri.AbsoluteUri, error);
            }
            else
            {
                url = string.Format("{0}#error={1}&state={2}", redirectUri.AbsoluteUri, error, state);
            }

            return new RedirectResult(url);
        }
    }
}
