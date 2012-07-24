using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Repositories.Sql.Configuration
{
    public class KeyMaterialConfiguration
    {
        [Key]
        public int Id { get; set; }

        public string SigningCertificate { get; set; }
        public string DecryptionCertificate { get; set; }
        public string RSASigningKey { get; set; }
    }
}
