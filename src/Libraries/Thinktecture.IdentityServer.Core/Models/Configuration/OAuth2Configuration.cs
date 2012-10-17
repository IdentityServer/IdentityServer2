/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class OAuth2Configuration : ProtocolConfiguration
    {
        [Display(Name = "Enable 'Resource Owner Password Credential' Flow", Description = "This flow is for trusted applications only. Users enter their credentials directly into the client.")]
        public bool EnableResourceOwnerFlow { get; set; }

        [Display(Name = "Enable 'Implicit' Flow", Description = "This flow is for native apps and client web applications.")]
        public bool EnableImplicitFlow { get; set; }

        [Display(Name = "Require Client Authentication", Description = "Specifies whether client have to authenticate to use the token endpoint.")]
        public bool RequireClientAuthentication { get; set; }
    }
}
