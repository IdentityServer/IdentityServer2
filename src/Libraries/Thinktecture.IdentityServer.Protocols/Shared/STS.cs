using System;
using System.IdentityModel;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.IO;
using System.Security.Claims;
using System.ServiceModel;
using System.Text;
using System.Xml;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer.TokenService;

namespace Thinktecture.IdentityServer.Protocols
{
    public class STS
    {
        SecurityTokenService _sts;

        public STS() : this(TokenServiceConfiguration.Current.CreateSecurityTokenService())
        { }

        public STS(SecurityTokenService sts)
        {
            if (sts == null)
            {
                throw new ArgumentNullException("sts");
            }

            _sts = sts;
        }

        public bool TryIssueToken(EndpointReference appliesTo, ClaimsPrincipal principal, string tokenType, out SecurityToken token)
        {
            token = null;

            var rst = new RequestSecurityToken
            {
                RequestType = RequestTypes.Issue,
                AppliesTo = appliesTo,
                KeyType = KeyTypes.Bearer,
                TokenType = tokenType
            };

            try
            {
                var rstr = _sts.Issue(principal, rst);
                token = rstr.RequestedSecurityToken.SecurityToken;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryIssueToken(EndpointReference appliesTo, ClaimsPrincipal principal, string tokenType, out TokenResponse response)
        {
            SecurityToken token = null;
            response = new TokenResponse { TokenType = tokenType };

            var result = TryIssueToken(appliesTo, principal, tokenType, out token);
            if (result == false)
            {
                return false;
            }

            if (tokenType == TokenTypes.JsonWebToken || tokenType == TokenTypes.SimpleWebToken)
            {
                var handler = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers[tokenType];
                response.TokenString = handler.WriteToken(token);
                response.ContentType = "text";
            }
            else
            {
                var handler = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers;
                var sb = new StringBuilder(128);
                handler.WriteToken(new XmlTextWriter(new StringWriter(sb)), token);

                response.ContentType = "text/xml";
                response.TokenString = sb.ToString();
            }

            return result;
        }
    }
}
