using System;
using Thinktecture.IdentityServer.Models.Configuration;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.Samples
{
    class ConfigurationRepository : IConfigurationRepository
    {
        Configuration _global = new Configuration();
        KeyMaterial _keys = new KeyMaterial();
        WSTrust _wstrust = new WSTrust();
        WSFederation _wsfed = new WSFederation();
        Diagnostics _diag = new Diagnostics();

        public GlobalConfiguration Global
        {
            get
            {
                return _global;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public KeyMaterialConfiguration Keys
        {
            get
            {
                return _keys;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public WSTrustConfiguration WSTrust
        {
            get
            {
                return _wstrust;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #region Not Implemented
        public IdentityServer.Models.Configuration.DiagnosticsConfiguration Diagnostics
        {
            get
            {
                return _diag;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IdentityServer.Models.Configuration.FederationMetadataConfiguration FederationMetadata
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

        public IdentityServer.Models.Configuration.OAuth2Configuration OAuth2
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

        public IdentityServer.Models.Configuration.SimpleHttpConfiguration SimpleHttp
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

        public bool SupportsWriteAccess
        {
            get { throw new NotImplementedException(); }
        }

        public IdentityServer.Models.Configuration.WSFederationConfiguration WSFederation
        {
            get
            {
                return _wsfed;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
