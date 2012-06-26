/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System;
using System.Configuration;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.WindowsAzure.ServiceRuntime;
using Thinktecture.IdentityServer.Helper;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories.WindowsAzure
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        #region Runtime
        public GlobalConfiguration Configuration
        {
            get
            {
                return new GlobalConfiguration
                {
                    SiteName = GetConfigurationString("SiteName"),
                    IssuerUri = GetConfigurationString("IssuerUri"),
                    IssuerContactEmail = GetConfigurationString("IssuerContactEmail"),

                    DefaultTokenType = GetConfigurationString("DefaultTokenType"),
                    DefaultTokenLifetime = GetConfigurationInteger("DefaultTokenLifetime"),
                    MaximumTokenLifetime = GetConfigurationInteger("MaximumTokenLifetime"),
                    SsoCookieLifetime = GetConfigurationInteger("SsoCookieLifetime"),

                    RequireSsl = GetConfigurationBoolean("RequireSsl"),
                    RequireEncryption = GetConfigurationBoolean("RequireEncryption"),

                    EnableClientCertificates = GetConfigurationBoolean("EnableClientCertificates"),
                    EnableDelegation = GetConfigurationBoolean("EnableDelegation"),

                    AllowKnownRealmsOnly = GetConfigurationBoolean("AllowKnownRealmsOnly"),
                    AllowReplyTo = GetConfigurationBoolean("AllowReplyTo"),
                    RequireReplyToWithinRealm = GetConfigurationBoolean("RequireReplyToWithinRealm"),
                    
                    EnableStrongEpiForSsl = GetConfigurationBoolean("EnableStrongEpiForSsl"),
                    EnableFederationMessageTracing = GetConfigurationBoolean("EnableFederationMessageTracing"),
                    EnforceUsersGroupMembership = GetConfigurationBoolean("EnforceUsersGroupMembership"),

                    RequireSignInConfirmation = GetConfigurationBoolean("RequireSignInConfirmation"),
                };
            }
        }

        public EndpointConfiguration Endpoints
        {
            get
            {
                return new EndpointConfiguration
                {
                    WSFederation = GetConfigurationBoolean("Endpoints__WSFederation"),
                    FederationMetadata = GetConfigurationBoolean("Endpoints__FederationMetadata"),
                    
                    WSTrustMessage = GetConfigurationBoolean("Endpoints__WSTrustMessage"),
                    WSTrustMixed = GetConfigurationBoolean("Endpoints__WSTrustMixed"),

                    SimpleHttp = GetConfigurationBoolean("Endpoints__SimpleHttp"),
                    OAuth2 = GetConfigurationBoolean("Endpoints__OAuth2"),
                    OAuthWRAP = GetConfigurationBoolean("Endpoints__WRAP"),
                    JSNotify = GetConfigurationBoolean("Endpoints__JSNotify"),

                    HttpPort = GetConfigurationInteger("HttpPort"),
                    HttpsPort = GetConfigurationInteger("HttpsPort")
                };
            }
        }

        public CertificateConfiguration SslCertificate
        {
            get
            {
                var dn = GetConfigurationString("SslCertificateDistinguishedName");

                var cert = new CertificateConfiguration
                {
                    SubjectDistinguishedName = dn,
                };

                if (!string.IsNullOrWhiteSpace(dn))
                {
                    cert.Certificate = GetCertificateFromStore(dn);
                }

                return cert;
            }
        }

        public CertificateConfiguration SigningCertificate
        {
            get
            {
                var dn = GetConfigurationString("SigningCertificateDistinguishedName");

                var cert = new CertificateConfiguration
                {
                    SubjectDistinguishedName = dn,
                };

                if (!string.IsNullOrWhiteSpace(dn))
                {
                    cert.Certificate = GetCertificateFromStore(dn);
                }

                return cert;
            }
        }

        private X509Certificate2 GetCertificateFromStore(string distinguishedName)
        {
            return X509Certificates.GetCertificateFromStore(distinguishedName);
        }


        private bool GetConfigurationBoolean(string name)
        {
            var value = GetConfigurationString(name);

            bool valueBool = false;
            if (bool.TryParse(value, out valueBool))
            {
                return valueBool;
            }

            Tracing.TraceEvent(TraceEventType.Error, "Error converting configuration value to boolean: " + name, true);
            throw new ConfigurationErrorsException(name);
        }

        private int GetConfigurationInteger(string name)
        {
            var value = GetConfigurationString(name);

            int valueInt = 0;
            if (int.TryParse(value, out valueInt))
            {
                return valueInt;
            }

            Tracing.TraceEvent(TraceEventType.Error, "Error converting configuration value to integer: " + name, true);
            throw new ConfigurationErrorsException(name);
        }

        private string GetConfigurationString(string name)
        {
            Tracing.Verbose("Retrieving configuration value for: " + name);

            try
            {
                return RoleEnvironment.GetConfigurationSettingValue(name);
            }
            catch (Exception ex)
            {
                Tracing.TraceEvent(TraceEventType.Error, "Error retrieving configuration value: " + ex.ToString(), true);
                throw;
            }
        }
        #endregion

        #region Management
        public bool SupportsWriteAccess
        {
            get { return false; }
        }

        public void UpdateConfiguration(GlobalConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public void UpdateEndpoints(EndpointConfiguration endpoints)
        {
            throw new NotImplementedException();
        }

        public void UpdateCertificates(string sslSubjectName, string signingSubjectName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
