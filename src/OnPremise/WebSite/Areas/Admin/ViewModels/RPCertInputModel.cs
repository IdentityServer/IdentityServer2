using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class RPCertInputModel : CertificateInputModel
    {
        public HttpPostedFileBase EncryptingCertificate { get; set; }
        public override HttpPostedFileBase File
        {
            get
            {
                return EncryptingCertificate;
            }
        }
   }
}