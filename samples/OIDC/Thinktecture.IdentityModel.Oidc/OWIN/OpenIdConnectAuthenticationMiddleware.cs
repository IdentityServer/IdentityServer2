using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Oidc.OWIN
{
    class OpenIdConnectAuthenticationMiddleware : AuthenticationMiddleware<OpenIdConnectAuthenticationOptions>
    {
        private readonly OpenIdConnectConfiguration _configuration;

        public OpenIdConnectAuthenticationMiddleware(
            OwinMiddleware next,
            OpenIdConnectAuthenticationOptions options) : base(next, options)
        {
            //_federationConfiguration = Options.FederationConfiguration ?? new FederationConfiguration(loadConfig: true);
        }

        protected override AuthenticationHandler<OpenIdConnectAuthenticationOptions> CreateHandler()
        {
            throw new NotImplementedException();
        }
    }
}
