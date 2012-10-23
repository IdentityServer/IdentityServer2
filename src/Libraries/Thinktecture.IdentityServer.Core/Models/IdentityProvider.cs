/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models
{
    public class IdentityProvider
    {
        [UIHint("HiddenInput")]
        public int ID { get; set; }

        [Required]
        [Display(Order=1, Name = "Identifier", Description = "Unique identifier of the identity provider.")]
        public string Name { get; set; }
        
        [Required]
        [Display(Order=2, Name = "Display Name", Description = "Descriptive Name of the identity provider (for logging).")]
        public string DisplayName { get; set; }
        
        [Display(Order=3,Name = "Enabled", Description = "Specifies whether this provider will used.")]
        public bool Enabled { get; set; }

        [Display(Order = 4, Name = "Include in Home Realm Discovery", Description = "Specifies whether this provider will be shown in the HRD screen.")]
        public bool ShowInHrdSelection { get; set; }

        [Required]
        [UIHint("IdentityProviderType")]
        [Display(Order = 5, Name = "Type", Description = "Specifies the type of the identity provider.")]
        public Models.IdentityProviderTypes Type { get; set; }

        [Display(Order = 6, Name = "WS-Federation Endpoint", Description = "Specifies the endpoint of for the WS-Federation protocol.")]
        [AbsoluteUri]
        public string WSFederationEndpoint { get; set; }

        [Required]
        [UIHint("Thumbprint")]
        [Display(Order=7,Name = "Issuer Thumbprint", Description = "Specifies the issuer thumbprint for X.509 certificate based signature validation.")]
        public string IssuerThumbprint { get; set; }

        [Display(Order = 8, Name = "Client ID", Description = "")]
        public string ClientID { get; set; }
        [Display(Order = 9, Name = "Client Secret", Description = "")]
        public string ClientSecret { get; set; }
        [AbsoluteUri]
        [Display(Order = 10, Name = "Authorization Url", Description = "")]
        public string AuthorizationUrl { get; set; }
        [AbsoluteUri]
        [Display(Order = 11, Name = "Token Url", Description = "")]
        public string TokenUrl { get; set; }

    }
}