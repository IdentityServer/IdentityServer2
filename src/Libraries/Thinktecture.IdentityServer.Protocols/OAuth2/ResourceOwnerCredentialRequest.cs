/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Formatting;

namespace Thinktecture.IdentityServer.Protocols.OAuth2
{
    public class ResourceOwnerCredentialRequest
    {
        public string GrantType { get; set; }
        public string Scope { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public static ResourceOwnerCredentialRequest Parse(NameValueCollection values)
        {
            var request = new ResourceOwnerCredentialRequest
            {
                GrantType = values["grant_type"],
                Scope = values["scope"],
                UserName = values["username"],
                Password = values["password"]
            };

            return request;
        }
    }
}