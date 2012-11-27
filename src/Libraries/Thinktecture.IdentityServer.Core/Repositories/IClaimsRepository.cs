/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.Security.Claims;
using Thinktecture.IdentityServer.TokenService;

namespace Thinktecture.IdentityServer.Repositories
{
    /// <summary>
    /// Repository for emitting claims into an outgoing token and claims metadata
    /// </summary>
    public interface IClaimsRepository
    {
        IEnumerable<Claim> GetClaims(ClaimsPrincipal principal, RequestDetails requestDetails);
        IEnumerable<string> GetSupportedClaimTypes();
    }
}
