/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Claims;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer
{
    public class ClaimsTransformer : ClaimsAuthenticationManager
    {
        [Import]
        public IUserRepository UserRepository { get; set; }

        public ClaimsTransformer()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (!incomingPrincipal.Identity.IsAuthenticated)
            {
                return base.Authenticate(resourceName, incomingPrincipal);
            }

            UserRepository.GetRoles(incomingPrincipal.Identity.Name).ToList().ForEach(role =>
                incomingPrincipal.Identities.First().AddClaim(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String, Constants.InternalIssuer)));

            return incomingPrincipal;
        }
    }
}