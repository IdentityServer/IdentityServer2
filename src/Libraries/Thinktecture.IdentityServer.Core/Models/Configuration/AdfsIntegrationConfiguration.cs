/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class AdfsIntegrationConfiguration : ProtocolConfiguration
    {
        [Display(Name = "ADFS WS-Trust Endpoint", Description = "Address of the UserNameMixed WS-Trust endpoint on ADFS")]
        public Uri UserNameMixedModeEndpoint { get; set; }
        
        [Display(Name = "ADFS Signing Certificate Thumbprint", Description = "Thumbprint of the ADFS signing certificate. Only needed when creating new tokens")]
        public string IssuerSigningThumbprint { get; set; }

        [Display(Name = "Create new Token", Description = "If enabled, a new JWT token will be created, if not the original ADFS SAML token will be passed through")]
        public bool CreateNewToken { get; set; }

        [Display(Name = "Symmetric signing key", Description = "The symmetric singing key for the newly created token, empty if you want to use the default signing key")]
        public string SymmetricSigningKey { get; set; }
    }
}
