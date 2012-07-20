using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class GlobalConfiguration
    {
        public String SiteName { get; set; }
        public String IssuerUri { get; set; }
        public String IssuerContactEmail { get; set; }
        public string DefaultWSTokenType { get; set; }
        public string DefaultHttpTokenType { get; set; }
        public int DefaultTokenLifetime { get; set; }
        public int MaximumTokenLifetime { get; set; }
        public int SsoCookieLifetime { get; set; }

        public Boolean RequireEncryption { get; set; }
        public Boolean RequireRelyingPartyRegistration { get; set; }
        public Boolean EnableClientCertificateAuthentication { get; set; }
        public Boolean EnforceUsersGroupMembership { get; set; }

        public int HttpPort { get; set; }
        public int HttpsPort { get; set; }
    }
}
