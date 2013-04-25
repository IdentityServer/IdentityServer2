/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.TokenService;

namespace Thinktecture.IdentityServer.Tests
{
    [TestClass]
    public class ReplyToHandlingTest
    {
        IConfigurationRepository repo;
        Request request;
        ClaimsPrincipal _alice;

        [TestInitialize]
        public void Setup()
        {
            repo = ConfigurationRepositoryFactory.Create(Constants.ConfigurationModes.LockedDownAllowReplyTo);
            request = new Request(repo, new TestRelyingPartyRepository(), null);
            _alice = PrincipalFactory.Create(Constants.Principals.AliceUserName);
        }

        [TestMethod]
        public void IgnoreReplyToForRegisteredRPwithReplyTo()
        {
            var rst = RstFactory.Create(Constants.Realms.ExplicitReplyTo);
            rst.ReplyTo = "http://foo";
            var details = request.Analyze(rst, _alice);

            // make sure reply to is from configuration
            Assert.IsTrue(details.IsReplyToFromConfiguration);

            // reply to 
            Assert.IsTrue(details.ReplyToAddressIsWithinRealm);
            Assert.AreEqual(rst.AppliesTo.Uri.AbsoluteUri, details.ReplyToAddress.AbsoluteUri);
        }

        [TestMethod]
        public void HonourReplyToForRegisteredRPwithoutReplyTo()
        {
            var rst = RstFactory.Create(Constants.Realms.SslEncryption);
            rst.ReplyTo = Constants.Realms.SslEncryption + "subrealm/";
            var details = request.Analyze(rst, _alice);

            // make sure reply to is from configuration
            Assert.IsFalse(details.IsReplyToFromConfiguration);

            // reply to 
            Assert.IsTrue(details.ReplyToAddressIsWithinRealm);
            Assert.AreEqual(rst.ReplyTo, details.ReplyToAddress.AbsoluteUri);
        }

        [TestMethod]
        public void DetectCrossRealmRedirect()
        {
            var rst = RstFactory.Create(Constants.Realms.SslEncryption);
            rst.ReplyTo = "http://foo/";
            var details = request.Analyze(rst, _alice);

            // make sure reply to is from configuration
            Assert.IsFalse(details.IsReplyToFromConfiguration);

            // reply to 
            Assert.IsFalse(details.ReplyToAddressIsWithinRealm);
            Assert.AreEqual(rst.ReplyTo, details.ReplyToAddress.AbsoluteUri);
        }
    }
}
