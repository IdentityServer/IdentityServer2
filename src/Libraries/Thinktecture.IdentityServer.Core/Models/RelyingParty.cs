/*
 * Copyright (c) Dominick Baier.  All rights reserved.
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
        //[Display(Name = "ID", Description = "")]
        public string Id { get; set; }
        
        [Display(Name = "Enabled", Description = "")]
        public bool Enabled { get; set; }
        
        [Required]
        [Display(Name = "Display Name", Description = "")]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Realm", Description = "")]
        public Uri Realm { get; set; }

        [Display(Name = "Reply To", Description = "URL to return once a token is issued.")]
        public Uri ReplyTo { get; set; }

        [Display(Order=10002, Name = "Encrypting Certificate", Description = "")]
        public X509Certificate2 EncryptingCertificate { get; set; }

        [UIHint("Base64")]
        [Display(Order = 10001, Name = "Symmetric Signing Key", Description = "Base64 encoded key used for symmetric signing of tokens.")]
        public byte[] SymmetricSigningKey { get; set; }

        [Display(Name = "Extra Data 1", Description = "")]
        public string ExtraData1 { get; set; }
        [Display(Name = "Extra Data 2", Description = "")]
        public string ExtraData2 { get; set; }
        [Display(Name = "Extra Data 3", Description = "")]
        public string ExtraData3 { get; set; }
    }
}