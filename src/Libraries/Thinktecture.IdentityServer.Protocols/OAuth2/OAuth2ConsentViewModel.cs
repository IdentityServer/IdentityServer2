using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Protocols.OAuth2
{
    public class OAuth2ConsentViewModel
    {
        public string ResourceName { get; set; }
        public string ResourceUri { get; set; }
        public string ClientName { get; set; }
    }
}
