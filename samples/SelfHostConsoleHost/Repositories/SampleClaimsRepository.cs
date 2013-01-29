using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.TokenService;

namespace SelfHostConsoleHost
{
    class SampleClaimsRepository : IClaimsRepository
    {
        public IEnumerable<Claim> GetClaims(ClaimsPrincipal principal, RequestDetails requestDetails)
        {
            return new List<Claim>
            {
                principal.Identities.First().FindFirst(ClaimTypes.Name),
                new Claim("http://claims/custom/foo", "bar")
            };
        }

        public IEnumerable<string> GetSupportedClaimTypes()
        {
            throw new NotImplementedException();
        }
    }
}
