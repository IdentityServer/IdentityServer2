/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Thinktecture.IdentityModel;

namespace Thinktecture.IdentityServer.Tests
{
    internal static class PrincipalFactory
    {
        public static ClaimsPrincipal Create(string type)
        {
            switch (type)
            {
                case Constants.Principals.AliceUserName:
                    return Create("Alice", AuthenticationMethods.Password);
                case Constants.Principals.Anonymous:
                    return CreateAnonymous();
                default:
                    throw new ArgumentException("type");
            }
        }

        public static ClaimsPrincipal Create(string userName, string authenticationMethod)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.AuthenticationMethod, authenticationMethod)
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationMethod));
        }

        public static ClaimsPrincipal CreateAnonymous()
        {
            return Principal.Anonymous;
        }
    }
}
