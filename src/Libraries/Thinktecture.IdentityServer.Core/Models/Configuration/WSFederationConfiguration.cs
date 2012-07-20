using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class WSFederationConfiguration
    {
        public bool Enabled { get; set; }

        public bool EnableAuthentication { get; set; }
        public bool EnableFederation { get; set; }
        public bool EnableHrd { get; set; }

        public bool AllowReplyTo { get; set; }
        public Boolean RequireReplyToWithinRealm { get; set; }
        public Boolean RequireSslForReplyTo { get; set; }

        public Boolean RequireSignInConfirmation { get; set; }
    }
}
