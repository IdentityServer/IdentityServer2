using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    public class ClientCertificateController : Controller
    {
        [Import]
        public IClientCertificatesRepository clientCertificatesRepository { get; set; }

        public ClientCertificateController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }
        public ClientCertificateController(IClientCertificatesRepository clientCertificatesRepository)
        {
            this.clientCertificatesRepository = clientCertificatesRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
