/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System;
using System.IdentityModel.Protocols.WSTrust;
using System.Security.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Models.Configuration;
using Thinktecture.IdentityServer.TokenService;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Tests
{
    [TestClass]
    public class PolicyEnforcementTest
    {
        IConfigurationRepository repo;
        Request request;
        ClaimsPrincipal _alice;

        [TestInitialize]
        public void Setup()
        {
            repo = ConfigurationRepositoryFactory.Create(Constants.ConfigurationModes.LockedDown);
            request = new Request(repo, new TestRelyingPartyRepository(), null);
            _alice = PrincipalFactory.Create(Constants.Principals.AliceUserName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Validate_NoRealm()
        {
            var rst = new RequestSecurityToken { RequestType = RequestTypes.Issue };
            var details = request.Analyze(rst, _alice);

            // unknown realm
            request.Validate();
        }

        [TestMethod]
        public void Analyze_UnknownRealm()
        {
            var rst = RstFactory.Create(Constants.Realms.UnknownRealm);
            var details = request.Analyze(rst, _alice);

            // unknown realm
            Assert.IsFalse(details.IsKnownRealm);
        }

        [TestMethod]
        //[ExpectedException(typeof(InvalidScopeException))]
        [ExpectedException(typeof(Exception))]
        public void Validate_UnknownRealm()
        {
            var rst = RstFactory.Create(Constants.Realms.UnknownRealm);
            var details = request.Analyze(rst, _alice);

            // unknown realm
            request.Validate();
        }

        [TestMethod]
        public void Analyze_PlainTextNoEncryption()
        {
            var rst = RstFactory.Create(Constants.Realms.PlainTextNoEncryption);
            var details = request.Analyze(rst, _alice);

            // known realm, registered
            Assert.IsTrue(details.IsKnownRealm);
            Assert.AreEqual(rst.AppliesTo.Uri.AbsoluteUri, details.RelyingPartyRegistration.Realm.AbsoluteUri);
            
            // reply to 
            Assert.IsTrue(details.ReplyToAddressIsWithinRealm);
            Assert.AreEqual(rst.AppliesTo.Uri.AbsoluteUri, details.ReplyToAddress.AbsoluteUri);

            // security settings
            Assert.IsFalse(details.UsesSsl);
            Assert.IsFalse(details.UsesEncryption);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestException))]
        public void Validate_PlainTextNoEncryption()
        {
            var rst = RstFactory.Create(Constants.Realms.PlainTextNoEncryption);
            var details = request.Analyze(rst, _alice);
            
            request.Validate();
        }

        [TestMethod]
        public void Analyze_SslNoEncryption()
        {
            var rst = RstFactory.Create(Constants.Realms.SslNoEncryption);
            var details = request.Analyze(rst, _alice);

            // known realm, registered
            Assert.IsTrue(details.IsKnownRealm);
            Assert.AreEqual(rst.AppliesTo.Uri.AbsoluteUri, details.RelyingPartyRegistration.Realm.AbsoluteUri);

            // reply to
            Assert.IsTrue(details.ReplyToAddressIsWithinRealm);
            Assert.AreEqual(rst.AppliesTo.Uri.AbsoluteUri, details.ReplyToAddress.AbsoluteUri);
            
            // security settings
            Assert.IsTrue(details.UsesSsl);
            Assert.IsFalse(details.UsesEncryption);           
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestException))]
        public void Validate_SslNoEncryption()
        {
            var rst = RstFactory.Create(Constants.Realms.SslNoEncryption);
            var details = request.Analyze(rst, _alice);
            
            request.Validate();
        }

        [TestMethod]
        public void Analyze_PlainTextEncryption()
        {
            var rst = RstFactory.Create(Constants.Realms.PlainTextEncryption);
            var details = request.Analyze(rst, _alice);

            // known realm, registered
            Assert.IsTrue(details.IsKnownRealm);
            Assert.AreEqual(rst.AppliesTo.Uri.AbsoluteUri, details.RelyingPartyRegistration.Realm.AbsoluteUri);

            // reply to 
            Assert.IsTrue(details.ReplyToAddressIsWithinRealm);
            Assert.AreEqual(rst.AppliesTo.Uri.AbsoluteUri, details.ReplyToAddress.AbsoluteUri);

            // security settings
            Assert.IsFalse(details.UsesSsl);
            Assert.IsTrue(details.UsesEncryption);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestException))]
        public void Validate_PlainTextEncryption()
        {
            var rst = RstFactory.Create(Constants.Realms.PlainTextEncryption);
            var details = request.Analyze(rst, _alice);

            request.Validate();
        }

        [TestMethod]
        public void Analyze_SslEncryption()
        {
            var rst = RstFactory.Create(Constants.Realms.SslEncryption);
            var details = request.Analyze(rst, _alice);

            // known realm, registered
            Assert.IsTrue(details.IsKnownRealm);
            Assert.AreEqual(rst.AppliesTo.Uri.AbsoluteUri, details.RelyingPartyRegistration.Realm.AbsoluteUri);

            // reply to 
            Assert.IsTrue(details.ReplyToAddressIsWithinRealm);
            Assert.AreEqual(rst.AppliesTo.Uri.AbsoluteUri, details.ReplyToAddress.AbsoluteUri);

            // security settings
            Assert.IsTrue(details.UsesSsl);
            Assert.IsTrue(details.UsesEncryption);
        }

        [TestMethod]
        public void Validate_SslEncryption()
        {
            var rst = RstFactory.Create(Constants.Realms.SslEncryption);
            var details = request.Analyze(rst, _alice);

            request.Validate();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestException))]
        public void Validate_SymmetricSignatureNoSigningKey()
        {
            var rst = RstFactory.Create(Constants.Realms.PlainTextNoEncryption);
            rst.TokenType = TokenTypes.SimpleWebToken;
            
            var details = request.Analyze(rst, _alice);

            request.Validate();
        }

        [TestMethod]
        public void Analyze_ReplyToShouldBeIgnored()
        {
            var rst = RstFactory.Create(Constants.Realms.SslEncryption);
            rst.ReplyTo = "http://foo";
            var details = request.Analyze(rst, _alice);

            // known realm, registered
            Assert.IsTrue(details.IsKnownRealm);
            Assert.AreEqual(rst.AppliesTo.Uri.AbsoluteUri, details.RelyingPartyRegistration.Realm.AbsoluteUri);

            // reply to 
            Assert.IsTrue(details.ReplyToAddressIsWithinRealm);
            Assert.AreEqual(rst.AppliesTo.Uri.AbsoluteUri, details.ReplyToAddress.AbsoluteUri);

            // security settings
            Assert.IsTrue(details.UsesSsl);
            Assert.IsTrue(details.UsesEncryption);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Analyze_AnonymousClientIdentity()
        {
            var rst = RstFactory.Create(Constants.Realms.UnknownRealm);
            var details = request.Analyze(rst, PrincipalFactory.Create(Constants.Principals.Anonymous));

            // unknown realm
            Assert.IsFalse(details.IsKnownRealm);
        }
    }
}
