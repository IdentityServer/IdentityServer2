using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Models.Configuration;

namespace Thinktecture.IdentityServer.Repositories.Configuration
{
    public interface IConfigurationRepository
    {
        bool SupportsWriteAccess { get; }

        GlobalConfiguration Global { get; set; }
        DiagnosticsConfiguration Diagnostics { get; set; }
        KeyMaterialConfiguration Keys { get; set; }

        WSFederationConfiguration WSFederation { get; set; }
        FederationMetadataConfiguration FederationMetadata { get; set; }
        WSTrustConfiguration WSTrust { get; set; }
        OAuth2Configuration OAuth2 { get; set; }
        OAuthWrapConfiguration OAuthWrap { get; set; }
    }
}
