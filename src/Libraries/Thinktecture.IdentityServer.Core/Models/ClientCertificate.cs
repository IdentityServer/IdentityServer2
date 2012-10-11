/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models
{
    public class ClientCertificate
    {
        [Required]
        [Display(Name = "Description", Description = "Name of the user to map the certificate to.")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "User Name", Description = "Name of the user to map the certificate to.")]
        public string UserName { get; set; }

        [UIHint("Thumbprint")]
        [Required]
        [Display(Name = "Client Certificate Thumbprint", Description = "Thumbprint of the client certificate.")]
        public string Thumbprint { get; set; }
    }
}
