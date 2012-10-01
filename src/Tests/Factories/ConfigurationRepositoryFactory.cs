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
using Thinktecture.IdentityServer.Models.Configuration;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Tests.Repositories;

namespace Thinktecture.IdentityServer.Tests
{
    internal static class ConfigurationRepositoryFactory
    {
        public static IConfigurationRepository Create(string mode)
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

        public static IConfigurationRepository CreateLockedDownConfiguration()
        {
            var repo = new InMemoryConfigurationRepository();

            repo.Global = new GlobalConfiguration
            {
                SiteName = "Unit Test",
                IssuerUri = "http://test.identityserver.thinktecture.com",
                
                DefaultTokenLifetime = 10,
                MaximumTokenLifetime = 24,
                DefaultWSTokenType = "urn:oasis:names:tc:SAML:2.0:assertion",
                
                EnableClientCertificateAuthentication = true,
                RequireRelyingPartyRegistration = true,
                RequireEncryption = true,
            };

            repo.WSFederation = new WSFederationConfiguration
            {
                AllowReplyTo = false,
                RequireReplyToWithinRealm = true,
                RequireSslForReplyTo = true
            };

            return repo;
        }

        private static IConfigurationRepository CreateLockedDownAllowReplyToConfiguration()
        {
            var repo = CreateLockedDownConfiguration();

            repo.WSFederation = new WSFederationConfiguration
            {
                AllowReplyTo = true,
                RequireReplyToWithinRealm = true,
                RequireSslForReplyTo = true
            };

            return repo;
        }
    }
}
