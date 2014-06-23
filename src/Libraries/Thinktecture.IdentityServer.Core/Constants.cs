/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

namespace Thinktecture.IdentityServer
{
    public static class Constants
    {
        public const string AuthenticationType = "IdentityServer";
        public const string InternalIssuer = "IdentityServerInternal";

        public static class Claims
        {
            public const string Base = "http://identityserver.thinktecture.com/claims/";

            public const string IdentityProvider = Base + "identityprovider";
            public const string Client = Base + "client";
            public const string Scope = Base + "scope";
        }

        public static class Actions
        {
            public const string Issue          = "Issue";
            public const string Administration = "Administration";
        }

        public static class Resources
        {
            // issue actions
            public const string WSFederation = "WSFederation";
            public const string SimpleHttp   = "SimpleHttp";
            public const string OAuth2       = "OAuth2";
            public const string WRAP         = "WRAP";
            public const string WSTrust      = "WSTrust";
            public const string JSNotify     = "JSNotify";

            // administration actions
            public const string General             = "General";
            public const string Configuration       = "Configuration";
            public const string RelyingParty        = "RelyingParty";
            public const string ServiceCertificates = "ServiceCertificates";
            public const string ClientCertificates  = "ClientCertificates";
            public const string Delegation          = "Delegation";
            public const string UI                  = "UI";
        }

        public static class Roles
        {
            public const string InternalRolesPrefix          = "IdentityServer";
            public const string Users                        = "Users";
            public const string Administrators               = "Administrators";

            public const string IdentityServerUsers          = InternalRolesPrefix + Users;
            public const string IdentityServerAdministrators = InternalRolesPrefix + Administrators;
        }

        public static class CacheKeys
        {
            public const string WSFedMetadata = "Cache_WSFedMetadata";
            public const string WSFedRPMetadata = "Cache_WSFedRPMetadata";
        }
    }
}
