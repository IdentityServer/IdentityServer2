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
        IEnumerable<Models.IdentityProvider> IIdentityProviderRepository.GetAll()
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                return entities.IdentityProviders.ToList().ToDomainModel();
            }
        }

        public bool TryGet(string name, out Models.IdentityProvider identityProvider)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                identityProvider = entities.IdentityProviders.Where(idp => idp.Name == name).FirstOrDefault().ToDomainModel();
                return (identityProvider != null);
            }
        }

        public void Add(Models.IdentityProvider item)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                entities.IdentityProviders.Add(item.ToEntity());
                entities.SaveChanges();
            }
        }

        public void Delete(string name)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var item = entities.IdentityProviders.Where(idp => idp.Name == name).FirstOrDefault();
                if (item != null)
                {
                    entities.IdentityProviders.Remove(item);
                    entities.SaveChanges();
                }
            }
        }

        public void Update(Models.IdentityProvider item)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var dbitem = entities.IdentityProviders.Where(idp => idp.Name == item.Name).FirstOrDefault();
                if (dbitem != null)
                {
                    dbitem.IssuerThumbprint = item.IssuerThumbprint;
                    dbitem.DisplayName = item.DisplayName;
                    dbitem.ShowInHrdSelection = item.ShowInHrdSelection;
                    dbitem.WSFederationEndpoint = item.WSFederationEndpoint;
                    entities.SaveChanges();
                }
            }
        }
    }
}
