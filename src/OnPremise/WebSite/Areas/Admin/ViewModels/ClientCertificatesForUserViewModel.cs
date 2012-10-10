using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class CertificateInputModel
    {
        public HttpPostedFileBase Cert { get; set; }

        string thumbprint;
        public string Thumbprint
        {
            get
            {
                if (Cert == null || Cert.ContentLength == 0) return null;

                if (thumbprint == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        this.Cert.InputStream.CopyTo(ms);
                        var bytes = ms.ToArray();
                        var val = new X509Certificate2(bytes);
                        thumbprint = val.Thumbprint;
                    }
                }
                return thumbprint;
            }
        }

    }

    public class ClientCertificatesForUserViewModel
    {
        private Repositories.IClientCertificatesRepository clientCertificatesRepository;
        private Repositories.IUserManagementRepository userManagementRepository;
        
        [Required]
        public string UserName { get; set; }
        public IEnumerable<ClientCertificate> Certificates { get; set; }

        public ClientCertificate NewCertificate { get; set; }

        public bool IsNew
        {
            get
            {
                return this.UserName == null;
            }
        }

        public IEnumerable<SelectListItem> AllUserNames { get; set; }

        public ClientCertificatesForUserViewModel(IClientCertificatesRepository clientCertificatesRepository, IUserManagementRepository userManagementRepository, string username)
        {
            this.clientCertificatesRepository = clientCertificatesRepository;
            this.userManagementRepository = userManagementRepository;
            var allnames =
                userManagementRepository.GetUsers()
                .Select(x => new SelectListItem
                {
                    Text = x
                }).ToList();
            allnames.Insert(0, new SelectListItem { Text = "-Choose-", Value = "" });
            this.AllUserNames = allnames;
            
            this.UserName = username;
            NewCertificate = new ClientCertificate { UserName = username };
            if (!IsNew)
            {
                var certs =
                        this.clientCertificatesRepository
                        .GetClientCertificatesForUser(this.UserName)
                            .ToArray();
                this.Certificates = certs;
            }
            else
            {
                this.Certificates = new ClientCertificate[0];
            }
        }
    }
}