/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.IdentityModel.Protocols.WSTrust;

namespace Thinktecture.IdentityServer.Tests
{
    internal static class RstFactory
    {
        public static RequestSecurityToken Create(string realm)
        {
            return new RequestSecurityToken
            {
                RequestType = RequestTypes.Issue,
                AppliesTo = new EndpointReference(realm)
            };
        }
    }
}
