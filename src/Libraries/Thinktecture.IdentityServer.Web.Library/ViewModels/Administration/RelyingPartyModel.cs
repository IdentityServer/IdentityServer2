/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Thinktecture.IdentityServer.Web.ViewModels.Administration
{
    public class RelyingPartyModel
    {
        public string Id { get; set; }

        [DisplayName("Relying Party Name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Realm URI")]
        [Required]
        public string Realm { get; set; }

        [DisplayName("ReplyTo URL (optional)")]
        public string ReplyTo { get; set; }

        [DisplayName("Encrypting Certificate Base64 String(optional)")]
        public string EncryptingCertificate { get; set; }

        [DisplayName("Encrypting Certificate Name")]
        public string EncryptingCertificateName { get; set; }

        [DisplayName("Symmetric Signing Key (optional)")]
        public string SymmetricSigningKey { get; set; }

        [DisplayName("Extra Data 1")]
        public string ExtraData1 { get; set; }

        [DisplayName("Extra Data 2")]
        public string ExtraData2 { get; set; }

        [DisplayName("Extra Data 3")]
        public string ExtraData3 { get; set; }

        public HttpPostedFileBase CertificateUpload { get; set; }
    }
}