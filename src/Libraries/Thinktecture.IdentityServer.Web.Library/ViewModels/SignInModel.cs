/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Thinktecture.IdentityServer.Protocols;

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

        [DisplayName("Remember me?")]
        public bool EnableSSO { get; set; }

        bool? isSigninRequest;
        public bool IsSigninRequest
        {
            get
            {
                if (isSigninRequest == null)
                {
                    var rp = new AuthenticationHelper().GetRelyingPartyDetailsFromReturnUrl(this.ReturnUrl);
                    isSigninRequest = rp != null;
                }
                return isSigninRequest.Value;
            }
            set
            {
                isSigninRequest = value;
            }
        }
        public string ReturnUrl { get; set; }
        public bool ShowClientCertificateLink { get; set; }
    }
}