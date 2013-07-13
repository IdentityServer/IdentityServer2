using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class Grant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string GrantId { get; set; }
        
        [Required]
        public string GrantType { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public IEnumerable<string> Scopes { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public DateTime Expiration { get; set; }

        public string RedirectUri { get; set; }
    }
}
