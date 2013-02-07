/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class AdfsIntegrationConfiguration : ProtocolConfiguration
    {
        // general settings - authentication

        [Display(Name = "Enable username/password authentication", Description = "Enables the OAuth2 to ADFS authentication bridge")]      
        public bool AuthenticationEnabled { get; set; }

        [Display(Name = "Pass-through authentication token", Description = "If enabled, the original token from ADFS will be passed back")]
        public bool PassThruAuthenticationToken { get; set; }

        [Display(Name = "Lifetime of the authentication token", Description = "Specifies the lifetime of the authentication token (in minutes)")]
        public int AuthenticationTokenLifetime { get; set; }

        
        // general settings - federation

        [Display(Name = "Enable delegation", Description = "Enables the OAuth2 to ADFS delegation bridge")]
        public bool FederationEnabled { get; set; }

        [Display(Name = "Pass-through delegation token", Description = "If enabled, the original token from ADFS will be passed back")]
        public bool PassThruFederationToken { get; set; }

        [Display(Name = "Lifetime of the delegation token", Description = "Specifies the lifetime of the delegation token (in minutes)")]
        public int FederationTokenLifetime { get; set; }


        // general settings

        [Display(Name = "Symmetric signing key", Description = "The symmetric singing key for the newly created token, empty if you want to use the default signing key")]
        public string SymmetricSigningKey { get; set; }        


        // adfs settings

        [Display(Name = "ADFS UserName Endpoint", Description = "Address of the ADFS UserNameMixed endoint.")]
        public string UserNameAuthenticationEndpoint { get; set; }

        [Display(Name = "ADFS Delegation Endpoint", Description = "Address of the ADFS federation endpoint (mixed/symmetric/basic256).")]
        public string FederationEndpoint { get; set; }

        [Display(Name = "ADFS Signing Certificate Thumbprint", Description = "Thumbprint of the ADFS signing certificate.")]
        public string IssuerThumbprint { get; set; }

        [Display(Name = "ADFS Issuer URI", Description = "ADFS Issuer URI")]
        public string IssuerUri { get; set; }

        [Display(Name = "ADFS Encryption certificate", Description = "ADFS Issuer URI")]
        public X509Certificate2 EncryptionCertificate { get; set; }
    }
}
