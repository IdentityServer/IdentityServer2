using Thinktecture.IdentityServer.Models.Configuration;

namespace Thinktecture.Samples
{
    class KeyMaterial : KeyMaterialConfiguration
    {
        public KeyMaterial()
        {
            SigningCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2("client.pfx", "abc!123");
        }
    }
}
