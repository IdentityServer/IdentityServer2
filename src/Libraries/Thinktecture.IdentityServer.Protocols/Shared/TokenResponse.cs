/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Thinktecture.IdentityServer.Protocols
{
    public class TokenResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }
    }
}