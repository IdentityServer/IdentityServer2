/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;

namespace Thinktecture.IdentityServer.Models
{
    public class OpenIdConnectClient
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public ClientSecretTypes ClientSecretType { get; set; }

        public string Name { get; set; }
        public OAuthFlows Flow { get; set; }
        public bool AllowRefreshToken { get; set; }
        public IEnumerable<string> RedirectUris { get; set; }
        public bool RequireConsent { get; set; }
    }

    public enum OAuthFlows
    {
        AuthorizationCode,
        Implicit,
        ClientCredentials,
        ResourceOwnPasswordCredential
    }

    public enum ClientSecretTypes
    {
        SharedSecret,
        ClientCertificateThumbprint
    }
}
