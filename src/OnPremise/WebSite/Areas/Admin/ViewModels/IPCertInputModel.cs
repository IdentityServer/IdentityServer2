using System.Web;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class IPCertInputModel : CertificateInputModel
    {
        public HttpPostedFileBase IssuerThumbprintFile { get; set; }
        public override HttpPostedFileBase File
        {
            get { return IssuerThumbprintFile; }
        }
        public override string Name
        {
            get { return "IssuerThumbprintFile"; }
        }
    }
}