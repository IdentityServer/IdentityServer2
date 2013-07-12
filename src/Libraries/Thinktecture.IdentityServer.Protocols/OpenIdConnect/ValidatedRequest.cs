using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Protocols.OpenIdConnect
{
    public class ValidatedRequest
    {
        public Client Client { get; set; }

        public string State { get; set; }

        public string RedirectUri { get; set; }

        public string ResponseType { get; set; }

        public string[] Scopes { get; set; }
    }
}
