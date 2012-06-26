/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

namespace Thinktecture.IdentityServer.Models
{
    public class ClientCertificate
    {
        public string UserName { get; set; }
        public string Thumbprint { get; set; }
        public string Description { get; set; }
    }
}
