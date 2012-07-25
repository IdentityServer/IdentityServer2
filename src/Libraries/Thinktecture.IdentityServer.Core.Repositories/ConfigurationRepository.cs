using System;
using System.Linq;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Repositories.Sql;
using Entities = Thinktecture.IdentityServer.Repositories.Sql;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        public bool SupportsWriteAccess
        {
            get { return true; }
        }

        public Models.Configuration.GlobalConfiguration Global
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.GlobalConfiguration.First<Entities.Configuration.GlobalConfiguration>();

                    // map to domain model
                    return entity.ToDomainModel();
                }
            }
            set
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.GlobalConfiguration.First<Entities.Configuration.GlobalConfiguration>();
                    entities.GlobalConfiguration.Remove(entity);

                    entities.GlobalConfiguration.Add(value.ToEntity());
                    entities.SaveChanges();
                }
            }
        }

        public Models.Configuration.DiagnosticsConfiguration Diagnostics
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Models.Configuration.KeyMaterialConfiguration Keys
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.Keys.FirstOrDefault<Entities.Configuration.KeyMaterialConfiguration>();

                    // map to domain model
                    return entity.ToDomainModel();
                }
            }
            set
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.Keys.FirstOrDefault<Entities.Configuration.KeyMaterialConfiguration>();

                    if (entity != null)
                    {
                        entities.Keys.Remove(entity);
                    }

                    entities.Keys.Add(value.ToEntity());
                    entities.SaveChanges();
                }
            }
        }

        public Models.Configuration.WSFederationConfiguration WSFederation
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.WSFederation.First<Entities.Configuration.WSFederationConfiguration>();

                    // map to domain model
                    return entity.ToDomainModel();
                }
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Models.Configuration.FederationMetadataConfiguration FederationMetadata
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.FederationMetadata.First<Entities.Configuration.FederationMetadataConfiguration>();

                    // map to domain model
                    return entity.ToDomainModel();
                }
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Models.Configuration.WSTrustConfiguration WSTrust
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.WSTrust.First<Entities.Configuration.WSTrustConfiguration>();

                    // map to domain model
                    return entity.ToDomainModel();
                }
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Models.Configuration.OAuth2Configuration OAuth2
        {
            get
            {
                using (var entities = IdentityServerConfigurationContext.Get())
                {
                    var entity = entities.OAuth2.First<Entities.Configuration.OAuth2Configuration>();

                    // map to domain model
                    return entity.ToDomainModel();
                }
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Models.Configuration.OAuthWrapConfiguration OAuthWrap
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
