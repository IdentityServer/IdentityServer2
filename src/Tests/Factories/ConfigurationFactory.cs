/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Tests
{
    internal static class ConfigurationFactory
    {
        public static GlobalConfiguration Create(string mode)
        {
            switch (mode)
            {
                case Constants.ConfigurationModes.LockedDown:
                    return CreateLockedDownConfiguration();
                case Constants.ConfigurationModes.LockedDownAllowReplyTo:
                    return CreateLockedDownAllowReplyToConfiguration();
                default:
                    throw new ArgumentException("mode");
            }
        }

        public static GlobalConfiguration CreateLockedDownConfiguration()
        {
            return new GlobalConfiguration
            {
                SiteName = "Unit Test",
                IssuerUri = "http://test.identityserver.thinktecture.com",
                
                DefaultTokenLifetime = 10,
                MaximumTokenLifetime = 24,
                DefaultTokenType = "urn:oasis:names:tc:SAML:2.0:assertion",

                EnableClientCertificates = true,
                EnableDelegation = false,

                AllowKnownRealmsOnly = true,
                AllowReplyTo = false,
                RequireReplyToWithinRealm = true,
                
                RequireEncryption = true,
                RequireSsl = true,
                RequireSignInConfirmation = false
            };
        }

        private static GlobalConfiguration CreateLockedDownAllowReplyToConfiguration()
        {
            return new GlobalConfiguration
            {
                SiteName = "Unit Test",
                IssuerUri = "http://test.identityserver.thinktecture.com",

                DefaultTokenLifetime = 10,
                MaximumTokenLifetime = 24,
                DefaultTokenType = "urn:oasis:names:tc:SAML:2.0:assertion",

                EnableClientCertificates = true,
                EnableDelegation = false,

                AllowKnownRealmsOnly = true,
                AllowReplyTo = true,
                RequireReplyToWithinRealm = true,

                RequireEncryption = true,
                RequireSsl = true,
                RequireSignInConfirmation = false
            };
        }
    }
}
