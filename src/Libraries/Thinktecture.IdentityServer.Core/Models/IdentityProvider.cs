/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models
{
    public class IdentityProvider
    {
        [Required]
        [Display(Name = "Identifier", Description = "Unique identifier of the identity provider.")]
        [Editable(false)]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Display Name", Description = "Descriptive Name of the identity provider (for logging).")]
        public string DisplayName { get; set; }
        
        [Display(Name = "Include in Home Realm Discovery", Description = "Specifies whether this provider will be shown in the HRD screen.")]
        public bool ShowInHrdSelection { get; set; }

        [Display(Name = "WS-Federation Endpoint", Description = "Specifies the endpoint of for the WS-Federation protocol.")]
        [UrlValidator]
        public string WSFederationEndpoint { get; set; }

        [Required]
        [UIHint("Thumbprint")]
        [Display(Name = "Issuer Thumbprint", Description = "Specifies the issuer thumbprint for X.509 certificate based signature validation.")]
        public string IssuerThumbprint { get; set; }
    }
}