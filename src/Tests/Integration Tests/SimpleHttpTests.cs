/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Web;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thinktecture.IdentityModel.Constants;

namespace Thinktecture.IdentityServer.Tests
{
    [TestClass]
    public class SimpleHttp
    {
        string baseAddress = Constants.Http.LocalBaseAddress;
        //string baseAddress = Constants.Http.CloudBaseAddress;
        
        string rp = Constants.Realms.LocalRP;

        [TestMethod]
        public void NoRealm()
        {
            var client = new HttpClient();

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
            Assert.AreEqual<string>(MediaTypeNames.Text.Xml, result.Content.Headers.ContentType.MediaType);
        }

        //[TestMethod]
        //public void ValidClientCertificateCredential()
        //{
        //    var values = new Dictionary<string, string>
        //    {
        //        { "realm", rp }
        //    };

        //    var handler = new WebRequestHandler();
        //    handler.ClientCertificates.Add(HttpClientFactory.GetValidClientCertificate());

        //    var client = new HttpClient(handler);
            
        //    var result = client.GetAsync(baseAddress + values.ToQueryString()).Result;

        //    Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, result.StatusCode);
        //    Assert.AreEqual<string>(MediaTypeNames.Text.Xml, result.Content.Headers.ContentType.MediaType);
        //}

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
            Assert.AreEqual<string>(MediaTypeNames.Text.Xml, result.Content.Headers.ContentType.MediaType);
            
            Trace.Write(XElement.Parse(result.Content.ReadAsStringAsync().Result).ToString());
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
            Assert.AreEqual<string>(MediaTypeNames.Text.Xml, result.Content.Headers.ContentType.MediaType);

            Trace.Write(XElement.Parse(result.Content.ReadAsStringAsync().Result).ToString());
        }


        [TestMethod]
        public void ValidUserNameCredentialSwt()
        {
            var values = new Dictionary<string, string>
            {
                { "realm", rp },
                { "tokentype", TokenTypes.SimpleWebToken }
            };

            var client = new HttpClient();
            client.SetBasicAuthenticationHeader(Constants.Credentials.ValidUserName, Constants.Credentials.ValidPassword);

            var result = client.GetAsync(baseAddress + values.ToQueryString()).Result;
            
            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual<string>(MediaTypeNames.Text.Plain, result.Content.Headers.ContentType.MediaType);

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
