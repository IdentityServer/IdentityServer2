/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class Endpoints
    {
        [Key]
        public string Name { get; set; }

        [Required]
        public Boolean WSFederation { get; set; }
        
        [Required]
        public Boolean WSTrustMessage { get; set; }
        
        [Required]
        public Boolean WSTrustMixed { get; set; }
        
        [Required]
        public Boolean SimpleHttp { get; set; }
        
        [Required]
        public Boolean FederationMetadata { get; set; }
        
        [Required]
        public Boolean OAuthWRAP { get; set; }

        [Required]
        public Boolean OAuth2 { get; set; }

        [Required]
        public Boolean JSNotify { get; set; }
        
        [Required]
        public int HttpPort { get; set; }
        
        [Required]
        public int HttpsPort { get; set; }
    }
}
