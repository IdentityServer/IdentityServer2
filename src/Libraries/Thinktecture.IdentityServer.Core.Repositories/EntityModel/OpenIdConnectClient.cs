using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class OpenIdConnectClientEntity
    {
        [Key]
        public string ClientId { get; set; }
        [Required]
        public string ClientSecret { get; set; }
        public ClientSecretTypes ClientSecretType { get; set; }
        [Required]
        public string Name { get; set; }

        // openid connect
        public OpenIdConnectFlows Flow { get; set; }
        public bool AllowRefreshToken { get; set; }
        public int AccessTokenLifetime { get; set; }
        public int RefreshTokenLifetime { get; set; }
        public bool RequireConsent { get; set; }
        public ICollection<OpenIdConnectClientRedirectUri> RedirectUris { get; set; }
    }

    public class OpenIdConnectClientRedirectUri
    {
        public int ID { get; set; }
        [Required]
        public string RedirectUri { get; set; }
    }
}