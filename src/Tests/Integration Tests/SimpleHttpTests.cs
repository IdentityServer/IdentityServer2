/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web;
using Thinktecture.IdentityModel.Constants;

namespace Thinktecture.IdentityServer.Tests
{
    [TestClass]
    public class SimpleHttp
    {
        string baseAddress = Constants.Http.LocalBaseAddress;
        //string baseAddress = Constants.Http.CloudBaseAddress;
        
        string rp = Constants.Realms.TestRPSymmetric;

        [TestMethod]
        public void NoRealm()
        {
            var client = new HttpClient();
            client.SetBasicAuthenticationHeader(Constants.Credentials.ValidUserName, Constants.Credentials.ValidPassword);

            var result = client.GetAsync(new Uri(baseAddress)).Result;
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void MalformedRealm()
        {
            var values = new Dictionary<string, string>
            {
                { "realm", "malformed" }
            };

            var client = new HttpClient();
            client.SetBasicAuthenticationHeader(Constants.Credentials.ValidUserName, Constants.Credentials.ValidPassword);

            var result = client.GetAsync(baseAddress + values.ToQueryString()).Result;
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void NoCredentials()
        {
            var values = new Dictionary<string, string>
            {
                { "realm", rp }
            };

            var client = new HttpClient();

            var result = client.GetAsync(baseAddress + values.ToQueryString()).Result;
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [TestMethod]
        public void ValidUserNameCredential()
        {
            var values = new Dictionary<string, string>
            {
                { "realm", rp }
            };

            var client = new HttpClient();
            client.SetBasicAuthenticationHeader(Constants.Credentials.ValidUserName, Constants.Credentials.ValidPassword);

            var result = client.GetAsync(baseAddress + values.ToQueryString()).Result;
            
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual<string>("application/json", result.Content.Headers.ContentType.MediaType);
        }

        [TestMethod]
        public void UnAuthorizedUser()
        {
            var values = new Dictionary<string, string>
            {
                { "realm", rp }
            };

            var client = new HttpClient();
            client.SetBasicAuthenticationHeader(Constants.Credentials.UnauthorizedUserName, Constants.Credentials.ValidPassword);

            var result = client.GetAsync(baseAddress + values.ToQueryString()).Result;
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [TestMethod]
        public void ValidUserNameCredentialSaml11()
        {
            var values = new Dictionary<string, string>
            {
                { "realm", rp },
                { "tokentype", TokenTypes.Saml11TokenProfile11 }
            };

            var client = new HttpClient();
            client.SetBasicAuthenticationHeader(Constants.Credentials.ValidUserName, Constants.Credentials.ValidPassword);

            var result = client.GetAsync(baseAddress + values.ToQueryString()).Result;
            
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual<string>("application/json", result.Content.Headers.ContentType.MediaType);

            Trace.WriteLine(HttpUtility.UrlDecode(result.Content.ReadAsStringAsync().Result));
        }

        [TestMethod]
        public void ValidUserNameCredentialSaml2()
        {
            var values = new Dictionary<string, string>
            {
                { "realm", rp },
                { "tokentype", TokenTypes.Saml2TokenProfile11 }
            };

            var client = new HttpClient();
            client.SetBasicAuthenticationHeader(Constants.Credentials.ValidUserName, Constants.Credentials.ValidPassword);

            var result = client.GetAsync(baseAddress + values.ToQueryString()).Result;
            
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual<string>("application/json", result.Content.Headers.ContentType.MediaType);

            Trace.WriteLine(HttpUtility.UrlDecode(result.Content.ReadAsStringAsync().Result));
        }


        [TestMethod]
        public void ValidUserNameCredentialJwt()
        {
            var values = new Dictionary<string, string>
            {
                { "realm", rp },
                { "tokentype", TokenTypes.JsonWebToken }
            };

            var client = new HttpClient();
            client.SetBasicAuthenticationHeader(Constants.Credentials.ValidUserName, Constants.Credentials.ValidPassword);

            var result = client.GetAsync(baseAddress + values.ToQueryString()).Result;
            
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual<string>("application/json", result.Content.Headers.ContentType.MediaType);

            Trace.WriteLine(HttpUtility.UrlDecode(result.Content.ReadAsStringAsync().Result));
        }

        [TestMethod]
        public void InvalidUserNameCredential()
        {
            var values = new Dictionary<string, string>
            {
                { "realm", rp }
            };

            var client = new HttpClient();
            client.SetBasicAuthenticationHeader(Constants.Credentials.ValidUserName, "invalid");

            var result = client.GetAsync(baseAddress + values.ToQueryString()).Result;
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.Unauthorized, result.StatusCode);
        }
    }
}
