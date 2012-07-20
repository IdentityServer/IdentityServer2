using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class KeyMaterialConfiguration
    {
        public X509Certificate2 SigningCertificate { get; set; }
        public X509Certificate2 DecryptionCertificate { get; set; }
        public RSA RSASigningKey { get; set; }
    }
}
