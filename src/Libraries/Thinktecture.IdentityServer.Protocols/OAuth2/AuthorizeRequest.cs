using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Protocols.OAuth2
{
    public class AuthorizeRequest
    {
        [Required]
        public string response_type { get; set; }
        
        [Required]
        public string client_id { get; set; }
        
        [Required]
        public string redirect_uri { get; set; }
        
        [Required]
        public string scope { get; set; }
        
        public string state { get; set; }
    }
}
