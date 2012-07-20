using System;
using Thinktecture.IdentityServer.Repositories.Configuration;

namespace Thinktecture.IdentityServer.Core.Repositories
{
    public class NewConfigurationRepository : INewConfigurationRepository
    {
        public bool SupportsWriteAccess
        {
            get { return true; }
        }

        public Models.Configuration.GlobalConfiguration Global
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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Models.Configuration.WSFederationConfiguration WSFederation
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

        public Models.Configuration.FederationMetadataConfiguration FederationMetadata
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

        public Models.Configuration.WSTrustConfiguration WSTrust
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

        public Models.Configuration.OAuth2Configuration OAuth2
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
