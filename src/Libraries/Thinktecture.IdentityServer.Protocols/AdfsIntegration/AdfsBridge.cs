using System;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using Thinktecture.IdentityModel.Tokens;
using Thinktecture.IdentityModel.WSTrust;

namespace Thinktecture.IdentityServer.Protocols.AdfsIntegration
{
    internal static class AdfsBridge
    {
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

        public static GenericXmlSecurityToken Delegate(string adfsEndpoint, SecurityToken token, string appliesTo)
        {
            return Delegate(adfsEndpoint, appliesTo, token, new ClientCredentials(), new WindowsWSTrustBinding(SecurityMode.TransportWithMessageCredential));    
        }

        public static GenericXmlSecurityToken Delegate(string adfsEndpoint, SecurityToken token, string appliesTo, string serviceAccountName, string serviceAccountPassword)
        {
            var credentials = new ClientCredentials();
            credentials.UserName.UserName = serviceAccountName;
            credentials.UserName.Password = serviceAccountPassword;

            return Delegate(adfsEndpoint, appliesTo, token, credentials, new UserNameWSTrustBinding(SecurityMode.TransportWithMessageCredential));   
        }

        public static GenericXmlSecurityToken Delegate(string adfsEndpoint, string appliesTo, SecurityToken token, ClientCredentials credentials, Binding binding)
        {
            var rst = new RequestSecurityToken
            {
                AppliesTo = new EndpointReference(appliesTo),
                RequestType = RequestTypes.Issue,

                ActAs = new SecurityTokenElement(token)
            };

            RequestSecurityTokenResponse rstr;
            return WSTrustClient.Issue(
                new EndpointAddress(adfsEndpoint),
                binding,
                credentials,
                rst,
                out rstr) as GenericXmlSecurityToken;
        }

        public static TokenResponse ConvertAuthenticationSamlToJwt(SecurityToken securityToken, string issuerThumbprint, string signingKey, string issuerUri)
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

        public static TokenResponse ConvertDelegationSamlToJwt(SecurityToken securityToken, string issuerThumbprint, string signingKey, string issuerUri)
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
            //configuration.IssuerNameRegistry = new TestIssuerNameRegistry();

            var handler = SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection(configuration);
            var identity = handler.ValidateToken(securityToken).First();
            return identity;
        }
    }
}
