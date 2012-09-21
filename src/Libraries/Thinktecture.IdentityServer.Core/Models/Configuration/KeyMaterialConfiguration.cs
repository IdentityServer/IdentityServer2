using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class KeyMaterialConfiguration
    {
        [Display(Name = "Signing Certificate", Description = "The X.509 certificate to sign outgoing SAML tokens")]
        public X509Certificate2 SigningCertificate { get; set; }

        [Display(Name = "Decryption Certificate", Description = "The X.509 certificate to decrypt incoming SAML tokens")]
        public X509Certificate2 DecryptionCertificate { get; set; }

        [Display(Name = "RSA Signing Key", Description = "The RSA key to sign outgoing JWT tokens")]
        public RSA RSASigningKey { get; set; }

        [Display(Name = "Symmetric Signing Key", Description = "The symmetric signing key to sign JWT tokens (e.g. for OAuth2 handshake)")]
        public string SymmetricSigningKey { get; set; }
    }
}
