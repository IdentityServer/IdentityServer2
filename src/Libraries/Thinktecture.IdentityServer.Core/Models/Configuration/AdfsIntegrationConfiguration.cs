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
        // OAuth2 to ADFS bridge - authentication

        [Display(Name = "Enable username/password authentication", Description = "Enables the OAuth2 to ADFS authentication bridge")]      
        public bool AuthenticationEnabled { get; set; }

        [Display(Name = "ADFS UserName Endpoint", Description = "Address of the UserNameMixed WS-Trust endpoint on ADFS for user authentication")]
        public string AuthenticationEndpoint { get; set; }

        [Display(Name = "Pass-through authentication token", Description = "If enabled, the original token from ADFS will be passed back")]
        public bool PassThruAuthenticationToken { get; set; }

        [Display(Name = "Lifetime of the authentication token", Description = "Specifies the lifetime of the authentication token (in minutes)")]
        public int AuthenticationTokenLifetime { get; set; }

        
        // OAuth2 to ADFS bridge - delegation

        [Display(Name = "Enable delegation", Description = "Enables the OAuth2 to ADFS delegation bridge")]
        public bool DelegationEnabled { get; set; }

        [Display(Name = "ADFS Delegation Endpoint", Description = "Address of the WindowsMixed WS-Trust endpoint on ADFS for delegation")]
        public string DelegationEndpoint { get; set; }

        [Display(Name = "Pass-through delegation token", Description = "If enabled, the original token from ADFS will be passed back")]
        public bool PassThruDelegationToken { get; set; }

        [Display(Name = "Lifetime of the delegation token", Description = "Specifies the lifetime of the delegation token (in minutes)")]
        public int DelegationTokenLifetime { get; set; }


        // Shared config options

        [Display(Name = "ADFS Signing Certificate Thumbprint", Description = "Thumbprint of the ADFS signing certificate. Only needed when creating new tokens")]
        public string IssuerThumbprint { get; set; }

        [Display(Name = "Symmetric signing key", Description = "The symmetric singing key for the newly created token, empty if you want to use the default signing key")]
        public string SymmetricSigningKey { get; set; }
    }
}
