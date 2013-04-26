/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Security.Cryptography.X509Certificates;

namespace Thinktecture.IdentityServer.Tests
{
    internal static class Constants
    {
        public static readonly string HostName;

        static Constants()
        {
            HostName = "idsrv.local";
        }

        public static class Http
        {
            public static readonly string LocalBaseAddress = "https://" + HostName + "/issue/simple";
            public static string CloudBaseAddress = "https://idp.thinktecture.com/issue/simple";
        }

        public static class Wrap
        {
            public static string LocalBaseAddress = "https://" + HostName + "/issue/wrap";
            public static string CloudBaseAddress = "https://idp.thinktecture.com/issue/wrap";
        }

        public static class OAuth2
        {
            public static string LocalBaseAddress = "https://" + HostName + "/issue/oauth2/token";
            public static string CloudBaseAddress = "https://idp.thinktecture.com/issue/oauth2";
        }

        public static class WSTrust
        {
            public static string LocalMixedUserName = "https://" + HostName + "/issue/wstrust/mixed/username";
            public static string CloudMixedUserName = "https://idp.thinktecture.com/issue/wstrust/mixed/username";

            public static string LocalMixedCertificate = "https://" + HostName + "/issue/wstrust/mixed/certificate";
            public static string CloudMixedCertificate = "https://idp.thinktecture.com/issue/wstrust/mixed/certificate";
        }

        public static class Realms
        {
            public const string TestRPSymmetric = "urn:test:symmetric";
            public const string TestRPAsymmetric = "urn:test:asymmetric";
            public const string TestRPEncryption = "urn:test:encryption";
 
            public const string DisabledRP = "https://test/disabled/";
            public const string UnknownRealm = "http://unknown/";
            public const string PlainTextNoEncryption = "http://server/noEncryption/";
            public const string SslNoEncryption = "https://server/noEncryption/";
            public const string PlainTextEncryption = "http://server/encryption/";
            public const string SslEncryption = "https://server/encryption/";
            public const string ExplicitReplyTo = "https://server/explicitreplyto/";
        }

        public static class Principals
        {
            public const string AliceUserName = "AliceUserName";
            public const string Anonymous = "Anoynmous";
        }

        public static class ConfigurationModes
        {
            public const string LockedDown = "LockedDown";
            public const string LockedDownAllowReplyTo = "LockedDownAllowReplyTo";
        }

        public static class Certificates
        {
            public const string DefaultEncryptionCertificateString = @"IAAAAAEAAADOBAAAMIIEyjCCArKgAwIBAgIKYQWg2wAAAAAAGDANBgkqhkiG9w0BAQUFADAbMRkwFwYDVQQDExBMZWFzdFByaXZpbGVnZUNBMB4XDTA4MDUxOTA1MzcxMFoXDTEzMDUxOTA1NDcxMFowEjEQMA4GA1UEAxMHU2VydmljZTCBnzANBgkqhkiG9w0BAQEFAAOBjQAwgYkCgYEAn4obl6lUFqeEw2pCgVWFmPGnRf7mChAXqH5lmfKOPRC6hh7V1MK/3d86TwHWpg8FKUS4PGICquqQwubI7E047QtTZEzXcebZCRQE9HZNhLROtiMPQB3XeljSRJKqhbiEhnMpFsPBBahHd0NEDmcPq+UCG++giXf36e9el6inw2UCAwEAAaOCAZswggGXMA4GA1UdDwEB/wQEAwIE8DATBgNVHSUEDDAKBggrBgEFBQcDATAdBgNVHQ4EFgQU7FMpPcpVwxRefnfwVnqDDELzpPswHwYDVR0jBBgwFoAUcIXV36QCEM2E4PAXL02aoLMnHU0wgaIGA1UdHwSBmjCBlzCBlKCBkaCBjoYraHR0cDovL2hvbWUvQ2VydEVucm9sbC9MZWFzdFByaXZpbGVnZUNBLmNybIYrZmlsZTovL2hvbWUvQ2VydEVucm9sbC9MZWFzdFByaXZpbGVnZUNBLmNybIYyaHR0cDovL3d3dy5sZWFzdHByaXZpbGVnZS5jb20vTGVhc3RQcml2aWxlZ2VDQS5jcmwwgYoGCCsGAQUFBwEBBH4wfDA8BggrBgEFBQcwAoYwaHR0cDovL2hvbWUvQ2VydEVucm9sbC9ob21lX0xlYXN0UHJpdmlsZWdlQ0EuY3J0MDwGCCsGAQUFBzAChjBmaWxlOi8vaG9tZS9DZXJ0RW5yb2xsL2hvbWVfTGVhc3RQcml2aWxlZ2VDQS5jcnQwDQYJKoZIhvcNAQEFBQADggIBADqlncvFFvHL5viU6sftGwsWD0u82Yh4nAmcLPt8MuExtB2oTYRNyQak8NJUp8NxtpTp1F7HqcG84bGJBRkRYX/JVZzrsf9EC9/dbovSxqtho4gz0Zo/+i/Bjo+52RRWu3GGCCEv4Dz0diH5tPO6Ma89A/bjpEI/oUjD92j0g9X3gZqPyw8XIXjaFurTd1HcuqAOTPC7XosYg1NH31ODhSaMiMfCYbvnUD1xB6PyzFQ8IaOvr54mguFOxMtz1c/1k5SgtkiXHoq9HLkY8xN3R8Yuk/0h2l60yu2yqrK87Zlo+Jx7+o1sxkHwI2cVleVFTI+5XKWW0u6yVGzqAEtY7P7SkuGSpN/qVDPM3rDqBf7WIeGmxwfKDkvAyYwBfyDR018tDf+MI/wDr+Xia72siGaZSIV9HilNxR2+KLy7AQVs0rQvc8xbUlcn9UXZZMtv3pItHRN/Y4XncTWDjNfwipec6oxBrW5MbzFZKzxJPnmC6365MKn152B8W4zQB6LxF6ZEwT2ZJ3Ln0bbQc1SEwZ4SIC3ROkw6Z1WVVGKxD6vukrwhOiRy7rhFoOowVXmVINWhRrvBA+h6FGBi/ciWhEQPzmhygXpd8+LVOwJGD8zWqmxHCY1+swizc3aStZSCDTJkjghn52DLbTqZbofbFY7keZi6XV92uBn1Z96nuLkM";
            public const string ValidClientCertificateName = "CN=Client";

            public static X509Certificate2 DefaultEncryptionCertificate = new X509Certificate2(Convert.FromBase64String(DefaultEncryptionCertificateString));
        }

        public static class Credentials
        {
            public const string ValidUserName = "bob";
            public const string UnauthorizedUserName = "unauthorized";

            public const string ValidPassword = "abc!123";

            public const string ValidClientId = "testclient";
            public const string ValidClientSecret = "secret";
        }
    }
}
