using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class ClientCertificateInputModel : CertificateInputModel
    {
        public HttpPostedFileBase Thumbprintfile { get; set; }
        public override HttpPostedFileBase File
        {
            get { return Thumbprintfile; }
        }
    }
}