using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class WSTrustConfiguration
    {
        public bool Enabled { get; set; }

        public bool EnableMessageSecurity { get; set; }
        public bool EnableMixedModeSecurity { get; set; }
        public bool EnableClientCertificateAuthentication { get; set; }
        public bool EnableFederatedAuthentication { get; set; }
        public Boolean EnableDelegation { get; set; }
        public Boolean EnableStrongEpiForSsl { get; set; }
    }
}
