/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Thinktecture.IdentityServer.Tests
{
    internal static class Extensions
    {
        public static void SetBasicAuthenticationHeader(this HttpClient client, string userName, string password)
        {
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string credential = String.Format("{0}:{1}", userName, password);

            var encoded = Convert.ToBase64String(encoding.GetBytes(credential));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);
        }

        public static void SetAccessToken(this HttpClient client, string token, string tokenType)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
        }

        public static string ToQueryString(this Dictionary<string, string> dictionary)
        {
            var sb = new StringBuilder(128);
            sb.Append("?");

            foreach (var entry in dictionary)
            {
                sb.AppendFormat("{0}={1}&", entry.Key, entry.Value);
            }

            return sb.ToString().TrimEnd('&');
        }
    }
}
