/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.ServiceModel.Security;
using Thinktecture.IdentityModel.Constants;

namespace Thinktecture.IdentityServer.Tests
{
    [TestClass]
    public class WSTrust
    {
        IWSTrustChannelContract _mixedUserNameClient;
        IWSTrustChannelContract _mixedCertificateClient;

        RequestSecurityToken _rst;

        string baseAddressUserName = Constants.WSTrust.LocalMixedUserName;
        //string baseAddressUserName = Constants.WSTrust.CloudMixedUserName;

        string baseAddressCertificate = Constants.WSTrust.LocalMixedCertificate;

        string rp = Constants.Realms.TestRPEncryption;

        [TestInitialize]
        public void Setup()
        {
            _mixedUserNameClient = WSTrustClientFactory.CreateMixedUserNameClient(
                Constants.Credentials.ValidUserName,
                Constants.Credentials.ValidPassword,
                baseAddressUserName);

            _mixedCertificateClient = WSTrustClientFactory.CreateMixedCertificateClient(
                Constants.Certificates.ValidClientCertificateName,
                baseAddressCertificate);

            _rst = new RequestSecurityToken
            {
                AppliesTo = new EndpointReference(rp),
                RequestType = RequestTypes.Issue,
                KeyType = KeyTypes.Symmetric
            };

        }

        [TestMethod]
        public void ValidUserNameCredentialSymmetric()
        {
            RequestSecurityTokenResponse rstr;
            var token = _mixedUserNameClient.Issue(_rst, out rstr);

            Assert.IsTrue(token != null);
        }

        [TestMethod]
        public void ValidClientCertificateCredentialSymmetric()
        {
            RequestSecurityTokenResponse rstr;
            var token = _mixedCertificateClient.Issue(_rst, out rstr);

            Assert.IsTrue(token != null);
        }

        [TestMethod]
        public void ValidUserNameCredentialSaml11Symmetric()
        {
            var rst = new RequestSecurityToken
            {
                AppliesTo = new EndpointReference(rp),
                RequestType = RequestTypes.Issue,
                KeyType = KeyTypes.Symmetric,

                TokenType = TokenTypes.Saml11TokenProfile11
            };

            RequestSecurityTokenResponse rstr;
            var token = _mixedUserNameClient.Issue(rst, out rstr) as GenericXmlSecurityToken;

            Assert.IsTrue(token != null);
            Assert.IsTrue(token.ProofToken != null);
            Assert.AreEqual(TokenTypes.Saml11TokenProfile11, rstr.TokenType);
        }

        [TestMethod]
        public void ValidUserNameCredentialSaml11Bearer()
        {
            var rst = new RequestSecurityToken
            {
                AppliesTo = new EndpointReference(rp),
                RequestType = RequestTypes.Issue,
                KeyType = KeyTypes.Bearer,

                TokenType = TokenTypes.Saml11TokenProfile11
            };

            RequestSecurityTokenResponse rstr;
            var token = _mixedUserNameClient.Issue(rst, out rstr) as GenericXmlSecurityToken;

            Assert.IsTrue(token != null);
            Assert.IsTrue(token.ProofToken == null);
            Assert.AreEqual(TokenTypes.Saml11TokenProfile11, rstr.TokenType);
        }

        [TestMethod]
        public void ValidUserNameCredentialSaml2Bearer()
        {
            var rst = new RequestSecurityToken
            {
                AppliesTo = new EndpointReference(rp),
                RequestType = RequestTypes.Issue,
                KeyType = KeyTypes.Bearer,

                TokenType = TokenTypes.Saml2TokenProfile11
            };

            RequestSecurityTokenResponse rstr;
            var token = _mixedUserNameClient.Issue(rst, out rstr) as GenericXmlSecurityToken;

            Assert.IsTrue(token != null);
            Assert.IsTrue(token.ProofToken == null);
            Assert.AreEqual(TokenTypes.Saml2TokenProfile11, rstr.TokenType);
        }

        [TestMethod]
        public void ValidUserNameCredentialSaml2Symmetric()
        {
            var rst = new RequestSecurityToken
            {
                AppliesTo = new EndpointReference(rp),
                RequestType = RequestTypes.Issue,
                KeyType = KeyTypes.Symmetric,

                TokenType = TokenTypes.Saml2TokenProfile11
            };

            RequestSecurityTokenResponse rstr;
            var token = _mixedUserNameClient.Issue(rst, out rstr) as GenericXmlSecurityToken;

            Assert.IsTrue(token != null);
            Assert.IsTrue(token.ProofToken != null);
            Assert.AreEqual(TokenTypes.Saml2TokenProfile11, rstr.TokenType);
        }

        [TestMethod]
        public void ValidUserNameCredentialJwtSymmetric()
        {
            var rst = new RequestSecurityToken
            {
                AppliesTo = new EndpointReference(rp),
                RequestType = RequestTypes.Issue,
                KeyType = KeyTypes.Symmetric,

                TokenType = TokenTypes.JsonWebToken
            };

            RequestSecurityTokenResponse rstr;
            var token = _mixedUserNameClient.Issue(rst, out rstr) as GenericXmlSecurityToken;

            Assert.IsTrue(token != null);
            Assert.IsTrue(token.ProofToken != null);
            Assert.AreEqual(TokenTypes.JsonWebToken, rstr.TokenType);
        }

        [TestMethod]
        public void ValidUserNameCredentialJwtBearer()
        {
            var rst = new RequestSecurityToken
            {
                AppliesTo = new EndpointReference(rp),
                RequestType = RequestTypes.Issue,
                KeyType = KeyTypes.Bearer,

                TokenType = TokenTypes.JsonWebToken
            };

            RequestSecurityTokenResponse rstr;
            var token = _mixedUserNameClient.Issue(rst, out rstr) as GenericXmlSecurityToken;

            Assert.IsTrue(token != null);
            Assert.IsTrue(token.ProofToken == null);
            Assert.AreEqual(TokenTypes.JsonWebToken, rstr.TokenType);
        }


        [TestMethod]
        [ExpectedException(typeof(MessageSecurityException))]
        public void InvalidUserNameCredentialSymmetric()
        {
            var client = WSTrustClientFactory.CreateMixedUserNameClient(
                Constants.Credentials.ValidUserName,
                "invalid",
                baseAddressUserName);

            RequestSecurityTokenResponse rstr;
            var token = client.Issue(_rst, out rstr);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void UnauthorizedUserSymmetric()
        {
            var client = WSTrustClientFactory.CreateMixedUserNameClient(
                Constants.Credentials.UnauthorizedUserName,
                Constants.Credentials.ValidPassword,
                baseAddressUserName);

            RequestSecurityTokenResponse rstr;
            var token = client.Issue(_rst, out rstr);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void NoRealm()
        {
            var rst = new RequestSecurityToken
            {
                RequestType = RequestTypes.Issue,
                KeyType = KeyTypes.Symmetric
            };

            RequestSecurityTokenResponse rstr;
            var token = _mixedUserNameClient.Issue(rst, out rstr);
        }
    }
}
