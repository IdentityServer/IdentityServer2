using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Repositories.Sql.Configuration
{
    public class KeyMaterialConfiguration
    {
        [Key]
        public int Id { get; set; }

        public string SigningCertificateName { get; set; }
        public string DecryptionCertificateName { get; set; }
        public string RSASigningKey { get; set; }
        public string SymmetricSigningKey { get; set; }
    }
}
