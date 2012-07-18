/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.Linq;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class IdentityProviderRepository : IIdentityProviderRepository
    {
        IEnumerable<Models.IdentityProvider> IIdentityProviderRepository.Get()
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                return entities.IdentityProviders.ToList().ToDomainModel();
            }
        }

        public bool TryGet(string identifier, out Models.IdentityProvider identityProvider)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                identityProvider = entities.IdentityProviders.Where(idp => idp.Identifier == identifier).FirstOrDefault().ToDomainModel();
                return (identityProvider != null);
            }
        }
    }
}
