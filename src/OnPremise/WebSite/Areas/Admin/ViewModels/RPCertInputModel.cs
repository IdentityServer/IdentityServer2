using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class RPCertInputModel : IValidatableObject
    {
        public HttpPostedFileBase EncryptingCertificate { get; set; }
        public bool? RemoveCert { get; set; }

        bool certProcessed;
        X509Certificate2 cert;
        public X509Certificate2 Cert
        {
            get
            {
                ProcessCert();
                return cert;
            }
        }

        private void ProcessCert()
        {
            if (certProcessed) return;
            certProcessed = true;

            if (EncryptingCertificate != null && EncryptingCertificate.ContentLength > 0)
            {
                using (var ms = new MemoryStream())
                {
                    EncryptingCertificate.InputStream.CopyTo(ms);
                    var bytes = ms.ToArray();
                    var val = new X509Certificate2(bytes);
                    cert = val;
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            try
            {
                ProcessCert();
            }
            catch
            {
                errors.Add(new ValidationResult("Error processing certificate.", new string[]{"EncryptingCertificate"}));
            }
            return errors;
        }
    }
}