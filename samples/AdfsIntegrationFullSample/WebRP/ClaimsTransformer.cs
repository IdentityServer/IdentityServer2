using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace WebRP
{
    public class ClaimsTransformer : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (!incomingPrincipal.Identity.IsAuthenticated)
            {
                return base.Authenticate(resourceName, incomingPrincipal);
            }

            var id = incomingPrincipal.Identities.First();
            id.AddClaim(new Claim("http://local/claims/foo", "foo"));
            id.AddClaim(new Claim("urn:bar", "bar"));
            return incomingPrincipal;
        }
    }
}