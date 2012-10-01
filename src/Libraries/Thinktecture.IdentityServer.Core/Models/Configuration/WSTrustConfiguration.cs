using System;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class WSTrustConfiguration : ProtocolConfiguration
    {
        [Display(Name = "Enable Message Security Endpoints", Description = "Enables the message security endpoints.")]
        public bool EnableMessageSecurity { get; set; }

        [Display(Name = "Enable Mixed Mode Security Endpoints", Description = "Enables the mixed mode security endpoints.")]
        public bool EnableMixedModeSecurity { get; set; }
        
        [Display(Name = "Enable Client Certificates Authentication", Description = "Enables client certificate based authentication.")]
        public bool EnableClientCertificateAuthentication { get; set; }
        
        [Display(Name = "Enable Federated Authentication", Description = "Enables federated authentication using a token from a trusted identity provider.")]
        public bool EnableFederatedAuthentication { get; set; }
        
        [Display(Name = "Enable Identity Delegation", Description = "Enables identity delegation.")]
        public Boolean EnableDelegation { get; set; }
    }
}
