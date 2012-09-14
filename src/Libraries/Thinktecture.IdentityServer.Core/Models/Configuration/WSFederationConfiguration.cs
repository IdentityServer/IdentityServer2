using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class WSFederationConfiguration : ProtocolConfiguration
    {
        [Display(Name = "Enable Authentication", Description = "")]
        public bool EnableAuthentication { get; set; }
        
        [Display(Name = "Enable Federation", Description = "")]
        public bool EnableFederation { get; set; }
        
        [Display(Name = "Enable HRD", Description = "Enables Home Realm Discovery (HRD).")]
        public bool EnableHrd { get; set; }

        [Display(Name = "Allow ReplyTo", Description = "")]
        public bool AllowReplyTo { get; set; }
        
        [Display(Name = "Require ReplyTo Within Realm", Description = "")]
        public Boolean RequireReplyToWithinRealm { get; set; }
        
        [Display(Name = "Require SSL For ReplyTo", Description = "")]
        public Boolean RequireSslForReplyTo { get; set; }
    }
}
