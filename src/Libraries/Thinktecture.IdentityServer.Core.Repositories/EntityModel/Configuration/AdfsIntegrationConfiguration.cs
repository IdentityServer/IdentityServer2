using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Repositories.Sql.Configuration
{
    public class AdfsIntegrationConfiguration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool Enabled { get; set; }


        // general settings - authentication

        public bool AuthenticationEnabled { get; set; }
        public bool PassThruAuthenticationToken { get; set; }
        public int AuthenticationTokenLifetime { get; set; }

        // general settings - federation

        //public bool FederationEnabled { get; set; }
        //public bool PassThruFederationToken { get; set; }
        //public int FederationTokenLifetime { get; set; }


        // general settings

        public string SymmetricSigningKey { get; set; }        


        // adfs settings

        public string UserNameAuthenticationEndpoint { get; set; }
        //public string FederationEndpoint { get; set; }
        public string IssuerThumbprint { get; set; }
        //public string IssuerUri { get; set; }
        //public X509Certificate2 EncryptionCertificate { get; set; }
    }
}
