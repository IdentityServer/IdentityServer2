using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Oidc.OWIN
{
    public class OpenIdConnectAuthenticationOptions : AuthenticationOptions
    {
        public OpenIdConnectAuthenticationOptions() : base("OpenIdConnect")
        {
            AuthenticationMode = AuthenticationMode.Active;
            SigninAsAuthenticationType = "OpenIDConnect";
        }

        public string ReturnPath { get; set; }
        public string SigninAsAuthenticationType { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }


        public Uri AuthorizeEndpoint { get; set; }

        public Uri RedirectUri { get; set; }
    }
}
