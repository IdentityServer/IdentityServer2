using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Repositories.Sql.Configuration
{
    public class AdfsIntegrationConfiguration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public bool AuthenticationEnabled { get; set; }
        public string AuthenticationEndpoint { get; set; }
        public bool PassThruAuthenticationToken { get; set; }
        public int AuthenticationTokenLifetime { get; set; }

        public bool DelegationEnabled { get; set; }
        public string DelegationEndpoint { get; set; }
        public bool PassThruDelegationToken { get; set; }
        public int DelegationTokenLifetime { get; set; }

        public string IssuerThumbprint { get; set; }
        public string SymmetricSigningKey { get; set; }
    }
}
