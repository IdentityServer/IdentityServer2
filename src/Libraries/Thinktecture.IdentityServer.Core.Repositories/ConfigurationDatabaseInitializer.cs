/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Constants;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class ConfigurationDatabaseInitializer : CreateDatabaseIfNotExists<IdentityServerConfigurationContext>
    {
        protected override void Seed(IdentityServerConfigurationContext context)
        {
            context.Global.Add(CreateGlobalConfiguration());
            context.Endpoints.Add(CreateEndpointConfiguration());
            CreateCertificateConfiguration().ForEach(c => context.Certificates.Add(c));

            // for test purposes
            CreateTestRelyingParties().ForEach(rp => context.RelyingParties.Add(rp));
            CreateTestIdentityProviders().ForEach(idp => context.IdentityProviders.Add(idp));


            base.Seed(context);
        }

        private List<RelyingParties> CreateTestRelyingParties()
        {
            return new List<RelyingParties>
            {
                new RelyingParties 
                {
                    Name = "Test (URL)",
                    Realm = "https://test/rp/",
                    SymmetricSigningKey = "3ihK5qGVhp8ptIk9+TDucXQW4Aaengg3d5m6gU8nzc8=",
                    EncryptingCertificate = "MIIFvTCCA6WgAwIBAgIKYQdskgAAAAAAFzANBgkqhkiG9w0BAQUFADAbMRkwFwYDVQQDExBMZWFzdFByaXZpbGVnZUNBMB4XDTA5MDUzMTEzNDcwM1oXDTE4MDIxNjIwMjYxNVowbzELMAkGA1UEBhMCREUxDjAMBgNVBAgTBUJhV3VlMRMwEQYDVQQHEwpIZWlkZWxiZXJnMRcwFQYDVQQKEw5MZWFzdFByaXZpbGVnZTERMA8GA1UECxMIUmVzZWFyY2gxDzANBgNVBAMTBnJvYWRpZTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAJT+YJNRbxKrnwluTK0cRN4Fv+W3ESJcqtp3n11aeyB3Oc6E/vWAKLKZjXH/myDtRsfGk2HL4vYovLk0OJWvemmcoardVIZ+IE3/nnzYmxHm4gSCgsRWB20++nQavIG4eEwywhI4OoiN79r1dDzhC/y7g3dOkyW5tYmSHsZpR6BIBX8uj03LkLvUhZ3gEXGSUiQ6lS8qnrbvfWU6tjhNRuTSFSLZBYIVYHy5E2zvT138mDJGAW20M+kAPBhGDid1qON7UygZoi948PM8skRtH6Z6KKLHaHx21z304AR3EnbMsaHFHbiQKjVK0aZtUZz5BHLYmVD89YzCkAZxbdWTMSECAwEAAaOCAa0wggGpMA4GA1UdDwEB/wQEAwIE8DATBgNVHSUEDDAKBggrBgEFBQcDATB4BgkqhkiG9w0BCQ8EazBpMA4GCCqGSIb3DQMCAgIAgDAOBggqhkiG9w0DBAICAIAwCwYJYIZIAWUDBAEqMAsGCWCGSAFlAwQBLTALBglghkgBZQMEAQIwCwYJYIZIAWUDBAEFMAcGBSsOAwIHMAoGCCqGSIb3DQMHMB0GA1UdDgQWBBT7hVq1UfcVMOdZoR8ZH99pXDuEhzAfBgNVHSMEGDAWgBRwhdXfpAIQzYTg8BcvTZqgsycdTTBDBgNVHR8EPDA6MDigNqA0hjJodHRwOi8vd3d3LmxlYXN0cHJpdmlsZWdlLmNvbS9MZWFzdFByaXZpbGVnZUNBLmNybDCBggYIKwYBBQUHAQEEdjB0MDgGCCsGAQUFBzAChixodHRwOi8vY2EvQ2VydEVucm9sbC9DQV9MZWFzdFByaXZpbGVnZUNBLmNydDA4BggrBgEFBQcwAoYsZmlsZTovL0NBL0NlcnRFbnJvbGwvQ0FfTGVhc3RQcml2aWxlZ2VDQS5jcnQwDQYJKoZIhvcNAQEFBQADggIBAHxY8mSBylSvf8z2ul1qzi453jK/LS5D7V1W1Zrd7VnN2zUpjQQuZbGmgCIjmU5O0qmr3dNppE9wmdLmXtgSw4hHjewiHCEyS+mx8YAXCfcCvFSIl0msnCWSlm+fMtnmeJGK8dpbhcokNaOukB2mOC6SX4lGMXTNxVzVzbpyAvteGMxd5WTkrBML75m/wSRq+V+Bo6qXFXbWfB4nFUh/NtAz/oupBKgf8EspsLSxYh5cKE7WJ4v9G+/7XT5bQ33XKc8MPkKZ0LWRvcFzaEBnl9Y8fWCvl5xgLgSPh9BQw18hf+abAAZX3mCZHgvchO8UwYATBgTKl/8Bzo5B21cPome5vneBcJNZhls8u+RKIB3Zf2aS/EniXXY/kZBdGdVVZ1/BHzBcN/E5tvKwZ/W0dhgE13NF9yjYSV3lIhwrqHoLlnnY38S+tIMgDistmI2EOgYiUEh7ZPDrgPFFX8iZJreDLXOLPrJ+SkFyFM2AvQKhAGvtX979pow3bt3ScxU25iLwZvIMTELi45T4NiiWN80YLlccsqIf2J+6w1q3tBLzDtPxWO510BWbffJvtwfO6+Z6EJ1Bt5CgbrPlcFBuXjOkY5kXShGbGqmqa77y+d4ZlIGO+MVUlunyxw4/8k3i26cPtyMz/GJUl3GlWWa+ThyJUUe4VhDa2hFOvrIj41cz"
                },
                new RelyingParties
                {
                    Name = "Test (URN)",
                    Realm = "urn:test:rp",
                    SymmetricSigningKey = "3ihK5qGVhp8ptIk9+TDucXQW4Aaengg3d5m6gU8nzc8=",
                    EncryptingCertificate = "MIIFvTCCA6WgAwIBAgIKYQdskgAAAAAAFzANBgkqhkiG9w0BAQUFADAbMRkwFwYDVQQDExBMZWFzdFByaXZpbGVnZUNBMB4XDTA5MDUzMTEzNDcwM1oXDTE4MDIxNjIwMjYxNVowbzELMAkGA1UEBhMCREUxDjAMBgNVBAgTBUJhV3VlMRMwEQYDVQQHEwpIZWlkZWxiZXJnMRcwFQYDVQQKEw5MZWFzdFByaXZpbGVnZTERMA8GA1UECxMIUmVzZWFyY2gxDzANBgNVBAMTBnJvYWRpZTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAJT+YJNRbxKrnwluTK0cRN4Fv+W3ESJcqtp3n11aeyB3Oc6E/vWAKLKZjXH/myDtRsfGk2HL4vYovLk0OJWvemmcoardVIZ+IE3/nnzYmxHm4gSCgsRWB20++nQavIG4eEwywhI4OoiN79r1dDzhC/y7g3dOkyW5tYmSHsZpR6BIBX8uj03LkLvUhZ3gEXGSUiQ6lS8qnrbvfWU6tjhNRuTSFSLZBYIVYHy5E2zvT138mDJGAW20M+kAPBhGDid1qON7UygZoi948PM8skRtH6Z6KKLHaHx21z304AR3EnbMsaHFHbiQKjVK0aZtUZz5BHLYmVD89YzCkAZxbdWTMSECAwEAAaOCAa0wggGpMA4GA1UdDwEB/wQEAwIE8DATBgNVHSUEDDAKBggrBgEFBQcDATB4BgkqhkiG9w0BCQ8EazBpMA4GCCqGSIb3DQMCAgIAgDAOBggqhkiG9w0DBAICAIAwCwYJYIZIAWUDBAEqMAsGCWCGSAFlAwQBLTALBglghkgBZQMEAQIwCwYJYIZIAWUDBAEFMAcGBSsOAwIHMAoGCCqGSIb3DQMHMB0GA1UdDgQWBBT7hVq1UfcVMOdZoR8ZH99pXDuEhzAfBgNVHSMEGDAWgBRwhdXfpAIQzYTg8BcvTZqgsycdTTBDBgNVHR8EPDA6MDigNqA0hjJodHRwOi8vd3d3LmxlYXN0cHJpdmlsZWdlLmNvbS9MZWFzdFByaXZpbGVnZUNBLmNybDCBggYIKwYBBQUHAQEEdjB0MDgGCCsGAQUFBzAChixodHRwOi8vY2EvQ2VydEVucm9sbC9DQV9MZWFzdFByaXZpbGVnZUNBLmNydDA4BggrBgEFBQcwAoYsZmlsZTovL0NBL0NlcnRFbnJvbGwvQ0FfTGVhc3RQcml2aWxlZ2VDQS5jcnQwDQYJKoZIhvcNAQEFBQADggIBAHxY8mSBylSvf8z2ul1qzi453jK/LS5D7V1W1Zrd7VnN2zUpjQQuZbGmgCIjmU5O0qmr3dNppE9wmdLmXtgSw4hHjewiHCEyS+mx8YAXCfcCvFSIl0msnCWSlm+fMtnmeJGK8dpbhcokNaOukB2mOC6SX4lGMXTNxVzVzbpyAvteGMxd5WTkrBML75m/wSRq+V+Bo6qXFXbWfB4nFUh/NtAz/oupBKgf8EspsLSxYh5cKE7WJ4v9G+/7XT5bQ33XKc8MPkKZ0LWRvcFzaEBnl9Y8fWCvl5xgLgSPh9BQw18hf+abAAZX3mCZHgvchO8UwYATBgTKl/8Bzo5B21cPome5vneBcJNZhls8u+RKIB3Zf2aS/EniXXY/kZBdGdVVZ1/BHzBcN/E5tvKwZ/W0dhgE13NF9yjYSV3lIhwrqHoLlnnY38S+tIMgDistmI2EOgYiUEh7ZPDrgPFFX8iZJreDLXOLPrJ+SkFyFM2AvQKhAGvtX979pow3bt3ScxU25iLwZvIMTELi45T4NiiWN80YLlccsqIf2J+6w1q3tBLzDtPxWO510BWbffJvtwfO6+Z6EJ1Bt5CgbrPlcFBuXjOkY5kXShGbGqmqa77y+d4ZlIGO+MVUlunyxw4/8k3i26cPtyMz/GJUl3GlWWa+ThyJUUe4VhDa2hFOvrIj41cz"

                }
            };
        }

        private List<IdentityProvider> CreateTestIdentityProviders()
        {
            return new List<IdentityProvider>
            {
                new IdentityProvider
                {
                    Name = "ADFS",
                    DisplayName = "LeastPrivilege ADFS Server",
                    Type = Models.IdentityProviderTypes.WS,
                    WSFederationEndpoint = "https://adfs.leastprivilege.vm/adfs/ls/",
                    IssuerThumbprint = "8EC7F962CC083FF7C5997D8A4D5ED64B12E4C174"
                }
            };
        }

        private static Global CreateGlobalConfiguration()
        {
            return new Global
            {
                Name = "Standard",

                SiteName = "thinktecture identity server for .NET 4.5",
                IssuerUri = "http://identityserver45.thinktecture.com/trust/changethis",
                IssuerContactEmail = "office@thinktecture.com",
                DefaultTokenType = TokenTypes.Saml2TokenProfile11,
                HttpTokenType = TokenTypes.JsonWebToken,
                DefaultTokenLifetime = 10,
                MaximumTokenLifetime = 24,
                SsoCookieLifetime = 10,

                RequireSsl = true,
                RequireEncryption = false,
                RequireReplyToWithinRealm = true,
                RequireSignInConfirmation = false,

                AllowKnownRealmsOnly = true,
                AllowReplyTo = false,

                EnableDelegation = false,
                EnableClientCertificates = false,
                EnableFederationMessageTracing = false,
                EnableStrongEpiForSsl = false,
                EnforceUsersGroupMembership = true,
            };
        }

        private static Endpoints CreateEndpointConfiguration()
        {
            return new Endpoints
            {
                Name = "Standard",

                WSFederation = true,
                WSFederationHrd = false,
                FederationMetadata = true,
                WSTrustMessage = false,
                WSTrustMixed = true,

                SimpleHttp = false,
                OAuthWRAP = false,
                OAuth2 = false,
                JSNotify = false,

                HttpPort = 80,
                HttpsPort = 443

            };
        }

        private static List<Certificates> CreateCertificateConfiguration()
        {
            return new List<Certificates>
            {
                new Certificates
                {
                    Name = "SSL",
                    SubjectDistinguishedName = ""
                },

                new Certificates
                {
                    Name = "SigningCertificate",
                    SubjectDistinguishedName = ""
                },
            };
        }
    }
}