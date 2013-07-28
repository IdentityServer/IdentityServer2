using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Oidc.OWIN
{
    public class OpenIdConnectAuthenticationHandler : AuthenticationHandler<OpenIdConnectAuthenticationOptions>
    {
        // per request init
        protected override Task InitializeCore()
        {
            return base.InitializeCore();
        }

        protected override Task<AuthenticationTicket> AuthenticateCore()
        {
            throw new NotImplementedException();
        }

        // if returns true - pipeline will end
        public override Task<bool> Invoke()
        {
            return base.Invoke();
        }

        protected override Task ApplyResponseChallenge()
        {
            return base.ApplyResponseChallenge();
        }
    }
}
