/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Web.ViewModels.Administration
{
    public class EditCertificatesModel
    {
        public List<string> AvailableCertificates { get; set; }

        public List<SelectListItem> AvailableCertificatesList
        {
            get
            {
                if (AvailableCertificates != null)
                {
                    return
                        (from c in AvailableCertificates
                         select new SelectListItem
                         {
                             Text = c,
                             Value = c
                         })
                        .ToList();
                }

                return null;
            }
        }

        [DisplayName("Current SSL Certificate (only needed for strong endpoint identities")]
        public string SslCertificate { get; set; }

        [DisplayName("Current Signing Certificate")]
        public string SigningCertificate { get; set; }

        public string UpdatedSslCertificate { get; set; }
        public string UpdatedSigningCertificate { get; set; }

        [DisplayName("Update")]
        public bool UpdateSslCertificate { get; set; }

        [DisplayName("Update")]
        public bool UpdateSigningCertificate { get; set; }
    }
}
