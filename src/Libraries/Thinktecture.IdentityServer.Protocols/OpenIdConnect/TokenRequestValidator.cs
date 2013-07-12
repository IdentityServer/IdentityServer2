using System;
using System.Security.Claims;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Protocols.OAuth2;

namespace Thinktecture.IdentityServer.Protocols.OpenIdConnect
{
    class TokenRequestValidator
    {
        public ValidatedRequest Validate(TokenRequest request, ClaimsPrincipal clientPrincipal)
        {
            var validatedRequest = new ValidatedRequest();

            // validate request model binding
            if (request == null)
            {
                throw new TokenRequestValidationException(
                    "Invalid request parameters.",
                    OAuth2Constants.Errors.InvalidRequest);
            }

            // grant type is required
            if (string.IsNullOrWhiteSpace(request.Grant_Type))
            {
                throw new TokenRequestValidationException(
                    "Missing grant_type",
                    OAuth2Constants.Errors.UnsupportedGrantType);
            }

            // check supported grant types
            if (request.Grant_Type == OAuth2Constants.GrantTypes.AuthorizationCode ||
                request.Grant_Type == OAuth2Constants.GrantTypes.RefreshToken)
            {
                validatedRequest.GrantType = request.Grant_Type;
                Tracing.Information("Grant type: " + validatedRequest.GrantType);
            }
            else
            {
                throw new TokenRequestValidationException(
                    "Invalid grant_type: " + request.Grant_Type,
                    OAuth2Constants.Errors.UnsupportedGrantType);
            }

            // validate client credentials
            var client = ValidateClient(clientPrincipal);
            if (client == null)
            {
                throw new TokenRequestValidationException(
                    "Invalid client: " + clientPrincipal.Identity.Name,
                    OAuth2Constants.Errors.InvalidClient);
            }

            validatedRequest.Client = client;
            Tracing.InformationFormat("Client: {0} ({1})",
                validatedRequest.Client.Name,
                validatedRequest.Client.ClientId);

            switch (request.Grant_Type)
            {
                case OAuth2Constants.GrantTypes.AuthorizationCode:
                    ValidateCodeGrant(validatedRequest, request);
                    break;
                case OAuth2Constants.GrantTypes.RefreshToken:
                    ValidateRefreshTokenGrant(validatedRequest, request);
                    break;
                default:
                    throw new TokenRequestValidationException(
                        "Invalid grant_type: " + request.Grant_Type,
                        OAuth2Constants.Errors.UnsupportedGrantType);
            }

            Tracing.Information("Token request validation successful.");
            return validatedRequest;

        }

        private Client ValidateClient(ClaimsPrincipal clientPrincipal)
        {
            throw new NotImplementedException();
        }

        private void ValidateRefreshTokenGrant(ValidatedRequest validatedRequest, TokenRequest request)
        {
            throw new NotImplementedException();
        }

        private void ValidateCodeGrant(ValidatedRequest validatedRequest, TokenRequest request)
        {
            throw new NotImplementedException();
        }

        private Client ValidateClient(string clientId, string clientSecret)
        {
            throw new NotImplementedException();
        }
    }
}
