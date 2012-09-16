using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class AdfsBridgeConfiguration : ProtocolConfiguration
    {
        public string MixedUserNameEndpointAddress { get; set; }
        public string IssuerThumbprint { get; set; }
        // test
    }
}
