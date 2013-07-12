using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer.Protocols.OAuth2;

namespace Thinktecture.IdentityServer.Protocols.OpenIdConnect
{
    public class TokenController : ApiController
    {
        public HttpResponseMessage Post(TokenRequest request)
        {
            Tracing.Start("OpenID Connect Token Endpoint");

            ValidatedRequest validatedRequest;

            try
            {
                var validator = new TokenRequestValidator();
                validatedRequest = validator.Validate(request, ClaimsPrincipal.Current);
            }
            catch (TokenRequestValidationException ex)
            {
                Tracing.Error("Aborting OAuth2 token request");
                return Request.CreateOAuthErrorResponse(ex.OAuthError);
            }

            // switch over the grant type
            if (validatedRequest.GrantType.Equals(OAuth2Constants.GrantTypes.AuthorizationCode))
            {
                return ProcessAuthorizationCodeRequest(validatedRequest);
            }
            else if (string.Equals(validatedRequest.GrantType, OAuth2Constants.GrantTypes.RefreshToken))
            {
                return ProcessRefreshTokenRequest(validatedRequest);
            }
            
            Tracing.Error("invalid grant type: " + request.Grant_Type);
            return Request.CreateOAuthErrorResponse(OAuth2Constants.Errors.UnsupportedGrantType);
        }

        private HttpResponseMessage ProcessRefreshTokenRequest(ValidatedRequest validatedRequest)
        {
            throw new NotImplementedException();
        }

        private HttpResponseMessage ProcessAuthorizationCodeRequest(ValidatedRequest validatedRequest)
        {
            throw new NotImplementedException();
        } 
    }
}
