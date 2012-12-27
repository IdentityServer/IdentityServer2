using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer.Models.Configuration;

namespace Thinktecture.Samples
{
    class Configuration : GlobalConfiguration
    {
        public Configuration()
        {
            this.IssuerUri = "https://samples.thinktecture.com/inmemoryhosting";
            this.DefaultWSTokenType = TokenTypes.Saml2TokenProfile11;
            this.DefaultHttpTokenType = TokenTypes.JsonWebToken;

            DefaultTokenLifetime = 1;
            MaximumTokenLifetime = 8;
        }
    }
}
