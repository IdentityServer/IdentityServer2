using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class ClientCertificatesViewModel
    {
        private Repositories.IClientCertificatesRepository clientCertificatesRepository;

        public ClientCertificateUserInputModel[] Users { get; set; }

        public ClientCertificatesViewModel()
        {

        }

        public ClientCertificatesViewModel(Repositories.IClientCertificatesRepository clientCertificatesRepository)
        {
            this.clientCertificatesRepository = clientCertificatesRepository;
            this.Users =
                clientCertificatesRepository.List(-1, -1)
                .Select(x => new ClientCertificateUserInputModel
                {
                    Username = x
                }).ToArray();
        }
    }

    public class ClientCertificateUserInputModel
    {
        public string Username { get; set; }
        [Editable(false)]
        public bool Delete { get; set; }
    }
}