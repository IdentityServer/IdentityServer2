using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Oidc.OWIN
{
    public class OpenIdConnectAuthenticationHandler : AuthenticationHandler
    {
        protected override Task<AuthenticationTicket> AuthenticateCore()
        {
            throw new NotImplementedException();
        }

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
