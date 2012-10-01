/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models
{
    public class DelegationSetting
    {
        [Required]
        [Display(Name = "Description", Description = "Name of the user to map the certificate to.")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "User Name", Description = "Name of the user where this mapping applies to.")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Description", Description = "Realm identifier where the identity can be delegated to.")]
        public Uri Realm { get; set; }
        
    }
}
