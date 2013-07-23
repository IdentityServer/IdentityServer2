/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.DataAnnotations;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class RelyingParties
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool Enabled { get; set; }
        
        [Required]
        public string Realm { get; set; }

        public TokenType? TokenType { get; set; }
        public int TokenLifeTime { get; set; }
        
        public string ReplyTo { get; set; }

        [MaxLength]
        public string EncryptingCertificate { get; set; }
        public string SymmetricSigningKey { get; set; }

        public string ExtraData1 { get; set; }
        public string ExtraData2 { get; set; }
        public string ExtraData3 { get; set; }
    }
}
