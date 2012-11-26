using System.Collections.Generic;
using System.Security.Claims;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.TokenService;

namespace Thinktecture.IdentityServer.Repositories
{
    public class PassThruTransformationRuleRepository : IClaimsTransformationRulesRepository
    {
        public IEnumerable<Claim> ProcessClaims(ClaimsPrincipal incomingPrincipal, IdentityProvider identityProvider, RequestDetails details)
        {
            return incomingPrincipal.Claims;
        }
    }
}