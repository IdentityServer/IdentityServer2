using System;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Repositories.Sql.Configuration
{
    public class GlobalConfiguration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public String SiteName { get; set; }
        
        [Required]
        public String IssuerUri { get; set; }

        [Required]
        public String IssuerContactEmail { get; set; }

        [Required]
        public string DefaultWSTokenType { get; set; }

        [Required]
        public string DefaultHttpTokenType { get; set; }

        [Required]
        public int DefaultTokenLifetime { get; set; }

        [Required]
        public int MaximumTokenLifetime { get; set; }

        [Required]
        public int SsoCookieLifetime { get; set; }

        [Required]
        public Boolean RequireEncryption { get; set; }

        [Required]
        public Boolean RequireRelyingPartyRegistration { get; set; }

        [Required]
        public Boolean EnableClientCertificateAuthentication { get; set; }

        [Required]
        public Boolean EnforceUsersGroupMembership { get; set; }

        [Required]
        public int HttpPort { get; set; }

        [Required]
        public int HttpsPort { get; set; }

        [Required]
        public bool DisableSSL { get; set; }

        public string PublicHostName { get; set; }
    }
}
