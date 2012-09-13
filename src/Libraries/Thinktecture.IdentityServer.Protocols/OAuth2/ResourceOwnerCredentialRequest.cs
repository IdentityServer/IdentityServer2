/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Formatting;

namespace Thinktecture.IdentityServer.Protocols.OAuth2
{
    public class ResourceOwnerCredentialRequest
    {
        [Required]
        public string GrantType { get; set; }
        public string Scope { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}