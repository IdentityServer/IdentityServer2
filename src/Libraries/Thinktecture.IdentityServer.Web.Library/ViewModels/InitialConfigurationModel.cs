/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public class InitialConfigurationModel
    {
        [DisplayName("Site name")]
        [Required]
        public string SiteName { get; set; }

        [DisplayName("Issuer URI")]
        [Required]
        public string IssuerUri { get; set; }

        [DisplayName("Signing Certificate")]
        [Required]
        public string SigningCertificate { get; set; }

        public List<string> AvailableCertificates { get; set; }

        [DisplayName("Create default roles and admin account?")]
        public bool CreateDefaultAccounts { get; set; }

        [DisplayName("User name")]
        public string UserName { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

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
    }
}