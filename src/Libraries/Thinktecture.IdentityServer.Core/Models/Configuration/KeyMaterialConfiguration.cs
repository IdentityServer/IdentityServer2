/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class KeyMaterialConfiguration
    {
        [Display(Name = "Signing Certificate", Description = "The X.509 certificate to sign outgoing SAML tokens")]
        [Required]
        public X509Certificate2 SigningCertificate { get; set; }

        [Display(Name = "Decryption Certificate", Description = "The X.509 certificate to decrypt incoming SAML tokens")]
        public X509Certificate2 DecryptionCertificate { get; set; }

        //[Display(Name = "RSA Signing Key", Description = "The RSA key to sign outgoing JWT tokens")]
        //public RSA RSASigningKey { get; set; }

        [Display(Name = "Symmetric Signing Key", Description = "Default symmetric signing key")]
        [Required]
        public string SymmetricSigningKey { get; set; }
    }
}
