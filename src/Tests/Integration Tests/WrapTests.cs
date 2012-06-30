/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thinktecture.IdentityModel.Clients;

namespace Thinktecture.IdentityServer.Tests
{
    [TestClass]
    public class WrapTests
    {
        WrapClient _client;
        string baseAddress = Constants.Wrap.LocalBaseAddress;
        string scope = Constants.Realms.TestRP;

        [TestInitialize]
        public void Setup()
        {
            _client = new WrapClient(new Uri(baseAddress));
        }

        // TODO
        //[TestMethod]
        //public void ValidUserNameCredential()
        //{
        //    var swt = _client.Issue(Constants.Credentials.ValidUserName, Constants.Credentials.ValidPassword, new Uri(scope));

        //    Assert.IsTrue(swt != null);

        //    var id = swt.ToClaimsIdentity();

        //    Assert.IsTrue(id.Claims.Count > 0);

        //    foreach (var claim in id.Claims)
        //    {
        //        Trace.WriteLine(string.Format("{0} ::: {1}", claim.ClaimType, claim.Value));
        //    }
        //}

        [TestMethod]
        public void InvalidUserNameCredential()
        {
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "wrap_name", Constants.Credentials.ValidUserName },
                    { "wrap_password", "invalid" },
                    { "wrap_scope", scope }
                });

            var client = new HttpClient();
            var result = client.PostAsync(new Uri(baseAddress), form).Result;

            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [TestMethod]
        public void UnauthorizedUser()
        {
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "wrap_name", Constants.Credentials.UnauthorizedUserName },
                    { "wrap_password", Constants.Credentials.ValidPassword },
                    { "wrap_scope", scope }
                });

            var client = new HttpClient();
            var result = client.PostAsync(new Uri(baseAddress), form).Result;

            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [TestMethod]
        public void NoRealm()
        {
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "wrap_name", Constants.Credentials.ValidUserName },
                    { "wrap_password", Constants.Credentials.ValidPassword }
                });

            var client = new HttpClient();
            var result = client.PostAsync(new Uri(baseAddress), form).Result;

            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void MalformedRealm()
        {
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "wrap_name", Constants.Credentials.ValidUserName },
                    { "wrap_password", Constants.Credentials.ValidPassword },
                    { "wrap_scope", "invalid" }
                });

            var client = new HttpClient();
            var result = client.PostAsync(new Uri(baseAddress), form).Result;

            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void NoCredentials()
        {
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "wrap_scope", scope }
                });

            var client = new HttpClient();
            var result = client.PostAsync(new Uri(baseAddress), form).Result;

            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.Unauthorized, result.StatusCode);
        }
    }
}
