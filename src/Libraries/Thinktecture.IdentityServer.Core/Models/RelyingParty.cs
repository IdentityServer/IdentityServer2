/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Thinktecture.IdentityServer.Models
{
    public class RelyingParty
    {
        [Required]
        [Editable(false)]
        public string Id { get; set; }
        
        [Display(Name = "Enabled", Description = "Enabled or disable this RP.")]
        public bool Enabled { get; set; }
        
        [Required]
        [Display(Name = "Display Name", Description = "Descriptive name (shows up in trace logs).")]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Realm/Scope Name", Description = "Realm/Scope identifier URI.")]
        [AbsoluteUri]
        public Uri Realm { get; set; }

        [Required]
        [Display(Name = "Token life time (in minutes)", Description = "Can be used to override the default token lifetime (a value of 0 uses the global default).")]
        public int TokenLifeTime { get; set; }

        [Display(Name = "Redirect URL", Description = "URL to return to once a token is issued.")]
        [UrlValidator]
        public Uri ReplyTo { get; set; }

        [Display(Order=10002, Name = "Encrypting Certificate", Description = "Optional X.509 certificate to encrypt outgoing tokens.")]
        public X509Certificate2 EncryptingCertificate { get; set; }

        [Display(Order = 10001, Name = "Symmetric Signing Key", Description = "Base64 encoded key used for symmetric signing of tokens.")]
        public byte[] SymmetricSigningKey { get; set; }

        [Display(Name = "Extra Data 1", Description = "Extra data you can associate with the RP. (1)")]
        public string ExtraData1 { get; set; }
        [Display(Name = "Extra Data 2", Description = "Extra data you can associate with the RP. (2)")]
        public string ExtraData2 { get; set; }
        [Display(Name = "Extra Data 3", Description = "Extra data you can associate with the RP. (3)")]
        public string ExtraData3 { get; set; }
    }
}