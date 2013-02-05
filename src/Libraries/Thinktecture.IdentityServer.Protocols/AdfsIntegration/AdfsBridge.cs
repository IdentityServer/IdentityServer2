using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using Thinktecture.IdentityModel.Tokens;
using Thinktecture.IdentityModel.WSTrust;

namespace Thinktecture.IdentityServer.Protocols.AdfsIntegration
{
    internal static class AdfsBridge
    {
        public static TokenResponse ConvertSamlToJwt(SecurityToken securityToken, string issuerThumbprint, string signingKey, string issuerUri)
        {
            var identity = AdfsBridge.ValidateSamlToken(
                securityToken,
                issuerThumbprint);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                SigningCredentials = new HmacSigningCredentials(signingKey),
                TokenIssuerName = issuerUri
            };

            var jwtHandler = new JsonWebTokenHandler();
            var jwt = jwtHandler.CreateToken(descriptor);

            return new TokenResponse
            {
                AccessToken = jwtHandler.WriteToken(jwt)
            };
        }

        public static ClaimsIdentity ValidateSamlToken(SecurityToken securityToken, string issuerThumbprint)
        {
            var registry = new ConfigurationBasedIssuerNameRegistry();
            registry.AddTrustedIssuer(issuerThumbprint, "ADFS");

            var configuration = new SecurityTokenHandlerConfiguration();
            configuration.AudienceRestriction.AudienceMode = AudienceUriMode.Never;
            configuration.CertificateValidationMode = X509CertificateValidationMode.None;
            configuration.RevocationMode = X509RevocationMode.NoCheck;
            configuration.CertificateValidator = X509CertificateValidator.None;
            configuration.IssuerNameRegistry = registry;

            var handler = SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection(configuration);
            var identity = handler.ValidateToken(securityToken).First();
            return identity;
        }

        public static GenericXmlSecurityToken Authenticate(string userName, string password, string appliesTo, Uri adfsEndpoint)
        {
            var credentials = new ClientCredentials();
            credentials.UserName.UserName = userName;
            credentials.UserName.Password = password;

            return WSTrustClient.Issue(
                new EndpointAddress(adfsEndpoint),
                new EndpointAddress(appliesTo),
                new UserNameWSTrustBinding(SecurityMode.TransportWithMessageCredential),
                credentials) as GenericXmlSecurityToken;
        }
    }
}
