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
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri RedirectUri { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
