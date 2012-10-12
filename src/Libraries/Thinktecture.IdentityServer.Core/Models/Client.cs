/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

namespace Thinktecture.IdentityServer.Models
{
    public class Client
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string RedirectUri { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
