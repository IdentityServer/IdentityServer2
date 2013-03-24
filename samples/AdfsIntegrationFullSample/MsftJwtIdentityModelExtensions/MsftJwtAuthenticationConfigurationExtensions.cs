using Microsoft.IdentityModel.Tokens.JWT;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Tokens.Http;

namespace MsftJwtIdentityModelExtensions
{
    public static class MsftJwtAuthenticationConfigurationExtensions
    {
        class JWTSecurityTokenHandlerWrapper : JWTSecurityTokenHandler
        {
            TokenValidationParameters validationParams;
            public JWTSecurityTokenHandlerWrapper(TokenValidationParameters validationParams)
            {
                this.validationParams = validationParams;
            }

            public override System.Collections.ObjectModel.ReadOnlyCollection<System.Security.Claims.ClaimsIdentity> ValidateToken(SecurityToken token)
            {
                var jwt = token as JWTSecurityToken;
                var list = new List<ClaimsIdentity>(this.ValidateToken(jwt, validationParams).Identities);
                return list.AsReadOnly();
            }
        }

        public static void AddMsftJsonWebToken(
            this AuthenticationConfiguration configuration, 
            string issuer, string audience, 
            X509Certificate2 signingCert,
            AuthenticationOptions options,
            AuthenticationScheme scheme)
        {
            var validationParameters = new TokenValidationParameters()
            {
                AllowedAudience = audience,
                SigningToken = new X509SecurityToken(signingCert),
                ValidIssuer = issuer,
                ValidateExpiration = true
            };
            
            var handler = new JWTSecurityTokenHandlerWrapper(validationParameters);

            configuration.AddMapping(new AuthenticationOptionMapping
            {
                TokenHandler = new SecurityTokenHandlerCollection { handler },
                Options = options,
                Scheme = scheme
            });
        }

        public static void AddMsftJsonWebToken(
            this AuthenticationConfiguration configuration,
            string issuer, string audience, 
            X509Certificate2 signingCert,
            string scheme)
        {
            configuration.AddMsftJsonWebToken(
                issuer,
                audience,
                signingCert,
                AuthenticationOptions.ForAuthorizationHeader(scheme),
                AuthenticationScheme.SchemeOnly(scheme));
        }

        public static void AddMsftJsonWebToken(
            this AuthenticationConfiguration configuration, 
            string issuer, string audience,
            X509Certificate2 signingCert)
        {
            configuration.AddMsftJsonWebToken(
                issuer,
                audience,
                signingCert,
                AuthenticationOptions.ForAuthorizationHeader(JwtConstants.Bearer),
                AuthenticationScheme.SchemeOnly(JwtConstants.Bearer));
        }
    }
}
