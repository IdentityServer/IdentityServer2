/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models
{
    public class Client
    {
        [UIHint("HiddenInput")]
        public int ID { get; set; }

        [Display(Name = "Name", Description = "Display name.")]
        public string Name { get; set; }

        [Display(Name = "Description", Description = "Description.")]
        public string Description { get; set; }
        
        [AbsoluteUri]
        [Display(Name = "Redirect URI", Description = "Redirect URI.")]
        public Uri RedirectUri { get; set; }

        [Display(Name = "Client ID", Description = "Client ID.")]
        public string ClientId { get; set; }

        [Display(Name = "Client Secret", Description = "Client secret.")]
        public string ClientSecret { get; set; }

        [Display(Name = "Native Client", Description = "Native Client.")]
        public bool NativeClient { get; set; }

        [Display(Name = "Allow Implicit Flow", Description = "Allow implicit flow.")]
        public bool AllowImplicitFlow { get; set; }

        [Display(Name = "Allow Resource Owner Flow", Description = "Allow Resource Owner Flow.")]
        public bool AllowResourceOwnerFlow { get; set; }

        [Display(Name = "Allow Code Flow", Description = "Allow code flow.")]
        public bool AllowCodeFlow { get; set; }
    }
}
