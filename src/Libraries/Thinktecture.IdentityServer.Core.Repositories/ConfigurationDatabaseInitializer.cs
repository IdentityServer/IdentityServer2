using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class ConfigurationDatabaseInitializer : CreateDatabaseIfNotExists<IdentityServerConfigurationContext>
    {
        protected override void Seed(IdentityServerConfigurationContext context)
        {
            context.Global.Add(CreateGlobalConfiguration());
            context.Endpoints.Add(CreateEndpointConfiguration());
            CreateCertificateConfiguration().ForEach(c => context.Certificates.Add(c));


            base.Seed(context);
        }

        private static Global CreateGlobalConfiguration()
        {
            return new Global
            {
                Name = "Standard",
                
                SiteName = "thinktecture identity server for .NET 4.5",
                IssuerUri = "http://identityserver45.thinktecture.com/trust/changethis",
                IssuerContactEmail = "office@thinktecture.com",
                DefaultTokenType = "urn:oasis:names:tc:SAML:2.0:assertion",
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