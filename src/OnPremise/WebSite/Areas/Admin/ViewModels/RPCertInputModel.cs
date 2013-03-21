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
        public override string Name
        {
            get { return "EncryptingCertificate"; }
        }
    }
}