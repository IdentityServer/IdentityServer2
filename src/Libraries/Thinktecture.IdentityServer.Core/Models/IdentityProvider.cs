/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models
{
    public class IdentityProvider : IValidatableObject
    {
        [UIHint("HiddenInput")]
        public int ID { get; set; }

        [Required]
        [Display(Order = 1, Name = "Identifier", Description = "Unique identifier of the identity provider.")]
        public string Name { get; set; }

        [Required]
        [Display(Order = 2, Name = "Display Name", Description = "Descriptive Name of the identity provider (for logging).")]
        public string DisplayName { get; set; }

        [Display(Order = 3, Name = "Enabled", Description = "Specifies whether this provider will used.")]
        public bool Enabled { get; set; }

        [Display(Order = 4, Name = "Include in Home Realm Discovery", Description = "Specifies whether this provider will be shown in the HRD screen.")]
        public bool ShowInHrdSelection { get; set; }

        [Required]
        [UIHint("Enum")]
        [Display(Order = 5, Name = "Type", Description = "Specifies the type of the identity provider.")]
        public Models.IdentityProviderTypes Type { get; set; }

        [Display(Order = 6, Name = "WS-Federation Endpoint", Description = "Specifies the endpoint of for the WS-Federation protocol.")]
        [AbsoluteUri]
        public string WSFederationEndpoint { get; set; }

        string _IssuerThumbprint;
        [UIHint("Thumbprint")]
        [Display(Order = 7, Name = "Issuer Thumbprint", Description = "Specifies the issuer thumbprint for X.509 certificate based signature validation.")]
        public string IssuerThumbprint
        {
            get
            {
                return _IssuerThumbprint;
            }
            set
            {
                _IssuerThumbprint = value;
                if (_IssuerThumbprint != null) _IssuerThumbprint = _IssuerThumbprint.Replace(" ", "");
            }
        }

        [Display(Order = 8, Name = "Client ID", Description = "")]
        public string ClientID { get; set; }
        [Display(Order = 9, Name = "Client Secret", Description = "")]
        public string ClientSecret { get; set; }
        [AbsoluteUri]
        [Display(Order = 10, Name = "Authorization Url", Description = "")]
        public string AuthorizationUrl { get; set; }
        [Display(Order = 11, Name = "Profile Type", Description = "")]
        [UIHint("Enum")]
        public OAuthProfileTypes? ProfileType { get; set; }
        [Display(Order = 12, Name = "Custom Profile Type", Description = "")]
        public string CustomProfileType { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (this.Type == IdentityProviderTypes.WSStar)
            {
                if (String.IsNullOrEmpty(this.WSFederationEndpoint))
                {
                    errors.Add(new ValidationResult("WS-Federation Endpoint is required.", new string[] { "WSFederationEndpoint" }));
                }
                if (String.IsNullOrEmpty(this.IssuerThumbprint))
                {
                    errors.Add(new ValidationResult("Issuer Thumbprint is required.", new string[] { "IssuerThumbprint" }));
                }
            }
            if (this.Type == IdentityProviderTypes.OAuth2)
            {
                if (String.IsNullOrEmpty(this.ClientID))
                {
                    errors.Add(new ValidationResult("Client ID is required.", new string[] { "ClientID" }));
                }
                if (String.IsNullOrEmpty(this.ClientSecret))
                {
                    errors.Add(new ValidationResult("Client Secret is required.", new string[] { "ClientSecret" }));
                }
                if (this.ProfileType == null)
                {
                    errors.Add(new ValidationResult("Profile Type is required.", new string[] { "ProfileType" }));
                }
            }

            return errors;
        }
    }
}