using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Oidc.OWIN
{
    public class OpenIdConnectAuthenticationMiddleware : AuthenticationMiddleware<OpenIdConnectAuthenticationOptions>
    {
        public OpenIdConnectAuthenticationMiddleware(
            OwinMiddleware next,
            OpenIdConnectAuthenticationOptions options) : base(next, options)
        { }

        protected override AuthenticationHandler<OpenIdConnectAuthenticationOptions> CreateHandler()
        {
            return new OpenIdConnectAuthenticationHandler();
        }
    }

    public class OpenIdConnectAuthenticationHandler : AuthenticationHandler<OpenIdConnectAuthenticationOptions>
    {
        protected override Task<Microsoft.Owin.Security.AuthenticationTicket> AuthenticateCoreAsync()
        {
            throw new NotImplementedException();
        }

        
    }

}
