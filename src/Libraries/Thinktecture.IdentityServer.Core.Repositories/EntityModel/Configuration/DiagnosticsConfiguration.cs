using System;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Repositories.Sql.Configuration
{
    public class DiagnosticsConfiguration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Boolean EnableFederationMessageTracing { get; set; }
    }
}
