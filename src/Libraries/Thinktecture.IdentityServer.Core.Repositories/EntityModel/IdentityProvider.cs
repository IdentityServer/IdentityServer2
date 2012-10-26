using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class IdentityProvider
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public int Type { get; set; }

        [Required]
        public bool ShowInHrdSelection { get; set; }

        public string WSFederationEndpoint { get; set; }
        public string IssuerThumbprint { get; set; }
        
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public int? OAuth2ProviderType { get; set; }

        public bool Enabled { get; set; }
    }
}