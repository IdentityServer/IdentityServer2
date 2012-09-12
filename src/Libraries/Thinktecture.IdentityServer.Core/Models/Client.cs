using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Models
{
    public class Client
    {
        public string Name { get; set; }
        public string ReturnUrl { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string ExtraData1 { get; set; }
        public string ExtraData2 { get; set; }
        public string ExtraData3 { get; set; }
    }
}
