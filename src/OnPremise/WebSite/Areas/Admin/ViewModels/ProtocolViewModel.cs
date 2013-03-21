using System.Web;
using Thinktecture.IdentityServer.Models.Configuration;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class ProtocolViewModel
    {
        public string ID { get; set; }
        public string DisplayName { get; set; }
        public ProtocolConfiguration Protocol { get; set; }
    }

    public class AdfsIntegrationCertInputModel : CertificateInputModel
    {
        public HttpPostedFileBase EncryptionCertificate { get; set; }
        public override HttpPostedFileBase File
        {
            get
            {
                return EncryptionCertificate;
            }
        }
        public override string Name
        {
            get { return "EncryptionCertificate"; }
        }
    }
}