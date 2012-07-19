using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class IdentityProvider
    {
        [Key]
        public string Name { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public bool ShowInHrdSelection { get; set; }

        public string WSFederationEndpoint { get; set; }
        public string IssuerThumbprint { get; set; }
    }
}