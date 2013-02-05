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
        // OAuth2 resource owner flow to ADFS feature

        [Display(Name = "Enable user authentication", Description = "Enables the OAuth2 resource owner flow to ADFS feature")]      
        public bool UserNameEnabled { get; set; }

        [Display(Name = "ADFS UserName Endpoint", Description = "Address of the UserNameMixed WS-Trust endpoint on ADFS for user authentication")]
        public string UserNameEndpoint { get; set; }

        [Display(Name = "Create new token", Description = "If enabled, a new JWT token will be created, if not the original ADFS SAML token will be passed through")]
        public bool CreateNewToken { get; set; }

        
        // OAuth2 assertion flow to ADFS feature

        [Display(Name = "Enable delegation", Description = "Enables the OAuth2 SAML assertion flow to ADFS delegation feature")]
        public bool DelegationEnabled { get; set; }

        [Display(Name = "ADFS Delegation Endpoint", Description = "Address of the WindowsMixed WS-Trust endpoint on ADFS for delegation")]
        public string DelegationEndpoint { get; set; }


        // Shared config options

        [Display(Name = "ADFS Signing Certificate Thumbprint", Description = "Thumbprint of the ADFS signing certificate. Only needed when creating new tokens")]
        public string IssuerSigningThumbprint { get; set; }

        [Display(Name = "Symmetric signing key", Description = "The symmetric singing key for the newly created token, empty if you want to use the default signing key")]
        public string SymmetricSigningKey { get; set; }
    }
}
