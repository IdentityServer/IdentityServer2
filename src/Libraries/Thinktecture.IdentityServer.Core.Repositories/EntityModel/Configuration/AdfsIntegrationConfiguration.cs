using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Repositories.Sql.Configuration
{
    public class AdfsIntegrationConfiguration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public bool UserNameEnabled { get; set; }
        public string UserNameEndpoint { get; set; }
        public bool CreateNewToken { get; set; }
        public bool DelegationEnabled { get; set; }
        public string DelegationEndpoint { get; set; }
        public string IssuerSigningThumbprint { get; set; }
        public string SymmetricSigningKey { get; set; }
    }
}
