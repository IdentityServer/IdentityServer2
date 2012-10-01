using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class ProtocolConfiguration
    {
        [Display(Order=1, Name = "Enabled", Description = "Enables this protocol.")]
        public bool Enabled { get; set; }
    }
}
