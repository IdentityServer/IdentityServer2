/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System.IdentityModel.Protocols.WSTrust;
using System.ServiceModel;

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
