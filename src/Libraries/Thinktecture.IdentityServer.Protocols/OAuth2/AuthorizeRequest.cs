/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */


namespace Thinktecture.IdentityServer.Protocols.OAuth2
{
    public class AuthorizeRequest
    {
        public string response_type { get; set; }
        public string client_id { get; set; }
        public string redirect_uri { get; set; }
        public string scope { get; set; }
        public string state { get; set; }
    }
}
