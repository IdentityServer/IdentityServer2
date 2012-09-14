using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class WSTrustConfiguration : ProtocolConfiguration
    {
        [Display(Name = "Enable Message Security", Description = "")]
        public bool EnableMessageSecurity { get; set; }
        [Display(Name = "Enable MixedMode", Description = "")]
        public bool EnableMixedModeSecurity { get; set; }
        [Display(Name = "Enable Client Certificates", Description = "")]
        public bool EnableClientCertificateAuthentication { get; set; }
        [Display(Name = "Enable Federated Authentication", Description = "")]
        public bool EnableFederatedAuthentication { get; set; }
        [Display(Name = "Enable Delegation", Description = "")]
        public Boolean EnableDelegation { get; set; }
    }
}
