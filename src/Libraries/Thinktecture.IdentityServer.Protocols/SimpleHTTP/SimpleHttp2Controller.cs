using System.ComponentModel.Composition;
using System.IdentityModel.Protocols.WSTrust;
using System.Security.Claims;
using System.Web.Http;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Security;

namespace Thinktecture.IdentityServer.Protocols.SimpleHTTP
{
    class SimpleHttp2Controller : ApiController
    {  
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public SimpleHttp2Controller()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public SimpleHttp2Controller(IConfigurationRepository configurationRepository)
        {
            ConfigurationRepository = configurationRepository;
        }

        public void Get([FromUri]string realm, [FromUri]string tokenType)
        {
            Tracing.Information("Simple HTTP endpoint called.");

            if (tokenType == null)
            {
                tokenType = ConfigurationRepository.Global.DefaultHttpTokenType;
            }

            Tracing.Verbose("Token type: " + tokenType);

            var appliesTo = new EndpointReference(realm);
            Tracing.Information("Simple HTTP endpoint called for realm: " + realm);


            var auth = new AuthenticationHelper();

            ClaimsPrincipal principal = null;
            if (!auth.TryGetPrincipalFromHttpRequest(Request, out principal))
            {
                Tracing.Error("no or invalid credentials found.");
                //return new UnauthorizedResult("Basic", UnauthorizedResult.ResponseAction.Send401);
            }

            if (!ClaimsAuthorize.CheckAccess(principal, Constants.Actions.Issue, Constants.Resources.SimpleHttp))
            {
                Tracing.Error("User not authorized");
                //return new UnauthorizedResult("Basic", UnauthorizedResult.ResponseAction.Send401);
            }

            TokenResponse tokenResponse;
            var sts = new STS();
            if (sts.TryIssueToken(appliesTo, principal, tokenType, out tokenResponse))
            {
                //return new SimpleHttpResult(tokenResponse.TokenString, tokenType);
            }
            else
            {
                //return new HttpStatusCodeResult(400);
            }
        }
    }
}
