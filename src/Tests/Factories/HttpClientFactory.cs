/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Thinktecture.IdentityServer.Helper;

namespace Thinktecture.IdentityServer.Tests
{
    internal static class HttpClientFactory
    {
        public static HttpClient Create(string baseAddress)
        {
            return new HttpClient { BaseAddress = new Uri(baseAddress) };
        }

        public static X509Certificate2 GetValidClientCertificate()
        {
            return X509Certificates.GetCertificateFromStore(
                    StoreLocation.CurrentUser,
                    StoreName.My,
                    X509FindType.FindBySubjectDistinguishedName,
                    Constants.Certificates.ValidClientCertificateName);
        }
    }
}
