using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class ProtocolConfiguration
    {
        [Display(Order=1, Name = "Enabled", Description = "Enables this protocol.")]
        public bool Enabled { get; set; }
    }
}
