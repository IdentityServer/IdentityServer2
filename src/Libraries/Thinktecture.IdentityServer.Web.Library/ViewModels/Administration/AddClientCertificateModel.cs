/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Thinktecture.IdentityServer.Web.ViewModels.Administration
{
    public class AddClientCertificateModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Description { get; set; }

        public string Thumbprint { get; set; }
        public HttpPostedFileBase CertificateUpload { get; set; }
        
    }
}