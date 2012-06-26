/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public class SignInModel
    {
        [Required]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Enable Single-Sign-On?")]
        public bool EnableSSO { get; set; }
    }
}