using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public class RelyingPartyViewModel
    {
        public string ID { get; set; }
        public string DisplayName { get; set; }
        public bool Enabled { get; set; }
    }
}