/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class AdfsIntegrationConfiguration : ProtocolConfiguration, IValidatableObject
    {
        // general settings - authentication

        [Display(Name = "Enable username/password authentication", Description = "Enables the OAuth2 to ADFS authentication bridge")]
        public bool UsernameAuthenticationEnabled { get; set; }
        
        [Display(Name = "Enable SAML authentication", Description = "Enables the OAuth2 to ADFS authentication bridge")]
        public bool SamlAuthenticationEnabled { get; set; }
        
        [Display(Name = "Enable JWT authentication", Description = "Enables the OAuth2 to ADFS authentication bridge")]
        public bool JwtAuthenticationEnabled { get; set; }

        [Display(Name = "Pass-through authentication token", Description = "If enabled, the original SAML token from ADFS will be passed back, otherwise the token will be converted to a JWT.")]
        public bool PassThruAuthenticationToken { get; set; }

        [Display(Name = "Lifetime of the token", Description = "Specifies the lifetime of the token (in minutes)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "AuthenticationTokenLifetime cannot be negative")]
        public int AuthenticationTokenLifetime { get; set; }

        // adfs settings

        [Display(Name = "ADFS UserName Endpoint", Description = "Address of the ADFS UserNameMixed endoint.")]
        public string UserNameAuthenticationEndpoint { get; set; }

        [Display(Name = "ADFS Federation Endpoint", Description = "Address of the ADFS federation endpoint (mixed/symmetric/basic256).")]
        public string FederationEndpoint { get; set; }

        [Display(Name = "ADFS Issuer URI", Description = "ADFS Issuer URI")]
        public string IssuerUri { get; set; }

        string _IssuerThumbprint;
        [Display(Name = "ADFS Signing Certificate Thumbprint", Description = "Thumbprint of the ADFS signing certificate.")]
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

        [Display(Name = "ADFS Encryption certificate", Description = "ADFS Issuer URI")]
        public X509Certificate2 EncryptionCertificate { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // common stuff
            if (this.Enabled)
            {
                if (this.UsernameAuthenticationEnabled ||
                    this.SamlAuthenticationEnabled ||
                    this.JwtAuthenticationEnabled)
                {
                    if (!this.PassThruAuthenticationToken &&
                        String.IsNullOrWhiteSpace(this.IssuerThumbprint))
                    {
                        yield return new ValidationResult("IssuerThumbprint required when PassThruAuthenticationToken is false.", new[] { "IssuerThumbprint" });
                    }
                }

                if (this.UsernameAuthenticationEnabled)
                {
                    if (String.IsNullOrWhiteSpace(this.UserNameAuthenticationEndpoint))
                    {
                        yield return new ValidationResult("UserNameAuthenticationEndpoint required when UsernameAuthenticationEnabled is enabled.", new[] { "UserNameAuthenticationEndpoint" });
                    }
                }

                if (this.SamlAuthenticationEnabled)
                {
                    if (String.IsNullOrWhiteSpace(this.IssuerThumbprint))
                    {
                        yield return new ValidationResult("IssuerThumbprint required when SamlAuthenticationEnabled is enabled.", new[] { "IssuerThumbprint" });
                    }

                    // EncryptionCertificate check done in controller

                    if (String.IsNullOrWhiteSpace(this.IssuerUri))
                    {
                        yield return new ValidationResult("IssuerUri required when SamlAuthenticationEnabled is enabled.", new[] { "IssuerUri" });
                    }
                    if (String.IsNullOrWhiteSpace(this.FederationEndpoint))
                    {
                        yield return new ValidationResult("FederationEndpoint required when SamlAuthenticationEnabled is enabled.", new[] { "FederationEndpoint" });
                    }
                }

                if (this.JwtAuthenticationEnabled)
                {
                    // EncryptionCertificate check done in controller

                    if (String.IsNullOrWhiteSpace(this.IssuerUri))
                    {
                        yield return new ValidationResult("IssuerUri required when JwtAuthenticationEnabled is enabled.", new[] { "IssuerUri" });
                    }
                    if (String.IsNullOrWhiteSpace(this.FederationEndpoint))
                    {
                        yield return new ValidationResult("FederationEndpoint required when JwtAuthenticationEnabled is enabled.", new[] { "FederationEndpoint" });
                    }
                }
            }
        }
    }
}
