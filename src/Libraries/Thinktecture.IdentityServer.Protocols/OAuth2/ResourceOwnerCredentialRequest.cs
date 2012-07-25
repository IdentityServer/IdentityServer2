/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Protocols.OAuth2
{
    public class ResourceOwnerCredentialRequest
    {
        [Required]
        public string Grant_Type { get; set; }

        [Required]
        public string Scope { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }       
    }
}