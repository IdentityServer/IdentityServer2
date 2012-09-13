using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class GlobalConfiguration
    {
        [Required]
        [Display(Name = "Site Name", Description = "Descriptive name of your STS.")]
        public String SiteName { get; set; }

        [Required]
        [Display(Name = "Site ID", Description = "Unique identifier for your STS. Must be a URI and will be the identifier issued in tokens and replying parties will use to identify your identity provider (this STS).")]
        public String IssuerUri { get; set; }

        [Display(Name = "Contact Email", Description = "Email of the contact person that administrates this STS.")]
        //[RegularExpression(@"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]\b", ErrorMessage = "{0} must be in the form of an email address.")]
        [Required]
        public String IssuerContactEmail { get; set; }

        [Display(Name = "Default WS Token", Description = "Default token type to be issues for WS-Federation and WS-Trust token requests.")]
        [Required]
        public string DefaultWSTokenType { get; set; }

        [Display(Name = "Default HTTP Token", Description = "Default token type to be issued for HTTP (non WS-*) token requests.")]
        [Required]
        public string DefaultHttpTokenType { get; set; }

        [Display(Name = "Default Token Lifetime", Description = "The default lifetime (in hours) of tokens issued from this STS.")]
        [Range(0, Int32.MaxValue)]
        public int DefaultTokenLifetime { get; set; }

        [Display(Name = "Maximum Token Lifetime", Description = "The maximum lifetime (in hours) of tokens issued from this STS if a relying party requests a lifetime longer than the default.")]
        [Range(0, Int32.MaxValue)]
        public int MaximumTokenLifetime { get; set; }

        [Display(Name = "SSO Duration", Description = "The duration (in hours) of the single sign-on cookies used for this web site.")]
        [Range(0, Int32.MaxValue)]
        public int SsoCookieLifetime { get; set; }

        [Display(Name = "Require Token Encryption", Description = "When enabled this setting requires tokens to be encrypted.")]
        public Boolean RequireEncryption { get; set; }

        [Display(Name = "Require RP Regitration", Description = "When enabled this will only issue tokens for relying parties that are registed with this STS.")]
        public Boolean RequireRelyingPartyRegistration { get; set; }

        [Display(Name = "Enable Client Certificates", Description = "Allow users to authenticate with client certificates.")]
        public Boolean EnableClientCertificateAuthentication { get; set; }

        [Display(Name = "Enforce Users in 'Users' Role", Description = "When enabled this setting will cause the STS to only issue tokens for users that are in the '" + Constants.Roles.IdentityServerUsers + "' role.")]
        public Boolean EnforceUsersGroupMembership { get; set; }

        [Display(Name = "HTTP Port", Description = "Port number the STS using for HTTP.")]
        [Range(0, Int32.MaxValue)]
        public int HttpPort { get; set; }

        [Display(Name = "HTTPS Port", Description = "Port number the STS is using for HTTPS.")]
        [Range(0, Int32.MaxValue)]
        public int HttpsPort { get; set; }
    }
}
