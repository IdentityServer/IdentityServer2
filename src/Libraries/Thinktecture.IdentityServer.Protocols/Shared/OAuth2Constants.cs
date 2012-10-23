using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Protocols
{
    public static class OAuth2Constants
    {
        public static class GrantTypes
        {
            public const string Password = "password";
            public const string AuthorizationCode = "authorization_code";
            public const string ClientCredentials = "client_credentials";
            public const string RefreshToken = "refresh_token";
        }

        public static class ResponseTypes
        {
            public const string Token = "token";
            public const string Code = "code";
        }

        public static class Errors
        {
            public const string Error = "error";
            public const string InvalidRequest = "invalid_request";
            public const string InvalidClient = "invalid_client";
            public const string InvalidGrant = "invalid_grant";
            public const string UnauthorizedClient = "unauthorized_client";
            public const string UnsupportedGrantType = "unsupported_grant_type";
            public const string UnsupportedResponseType = "unsupported_response_type";
            public const string InvalidScope = "invalid_scope";
        }
    }
}
