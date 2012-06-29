/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System;
using System.Collections.Specialized;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Tokens;

namespace Thinktecture.IdentityServer.OAuth
{
    /// <summary>
    /// Implements the OAuth WRAP protocol to request tokens from an issuer.
    /// </summary>
    public class WrapClient
    {
        Uri _issuerAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapClient"/> class.
        /// </summary>
        /// <param name="issuerAddress">The issuer address.</param>
        public WrapClient(Uri issuerAddress)
        {
            _issuerAddress = issuerAddress;
        }

        /// <summary>
        /// Requests an SWT Token using username/password credentials.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="scope">The requested scope.</param>
        /// <returns>The requested SWT token</returns>
        public SimpleWebToken Issue(string userName, string password, Uri scope)
        {
            var values = new NameValueCollection
                {
                    { "wrap_name", userName },
                    { "wrap_password", password },
                    { "wrap_scope", scope.AbsoluteUri }
                };

            return ResponseToSimpleWebToken(RequestToken(values));
        }

        /// <summary>
        /// Requests an SWT Token using an input SWT token.
        /// </summary>
        /// <param name="token">The input SWT token.</param>
        /// <param name="scope">The requested scope.</param>
        /// <returns>The requested SWT token</returns>
        public SimpleWebToken Issue(SimpleWebToken token, Uri scope)
        {
            return IssueAssertion(token.ToString(), "SWT", scope);
        }

        /// <summary>
        /// Requests an SWT Token using an input SAML token.
        /// </summary>
        /// <param name="token">The input SAML token.</param>
        /// <param name="scope">The requested scope.</param>
        /// <returns>The requested SWT token</returns>
        public SimpleWebToken Issue(SamlSecurityToken token, Uri scope)
        {
            var handler = new SamlSecurityTokenHandler();

            var sb = new StringBuilder(128);
            handler.WriteToken(XmlWriter.Create(new StringWriter(sb)), token);

            return IssueAssertion(sb.ToString(), "SAML", scope);
        }

        /// <summary>
        /// Requests an SWT Token using an input GenericXml SAML token.
        /// </summary>
        /// <param name="token">The input GenericXml SAML token.</param>
        /// <param name="scope">The requested scope.</param>
        /// <returns>The requested SWT token</returns>
        public SimpleWebToken Issue(GenericXmlSecurityToken token, Uri scope)
        {
            return IssueAssertion(token.TokenXml.OuterXml, "SAML", scope);
        }

        /// <summary>
        /// Requests an SWT Token using an input assertion.
        /// </summary>
        /// <param name="token">The assertion.</param>
        /// <param name="assertionFormat">The assertion format.</param>
        /// <param name="scope">The requested scope.</param>
        /// <returns>The requested SWT token</returns>
        public SimpleWebToken IssueAssertion(string token, string assertionFormat, Uri scope)
        {
            var values = new NameValueCollection
            {
                { "wrap_assertion_format", assertionFormat},
                { "wrap_assertion", token },
                { "wrap_scope", scope.AbsoluteUri }
            };

            return ResponseToSimpleWebToken(RequestToken(values));
        }

        private SimpleWebToken ResponseToSimpleWebToken(string response)
        {
            //string prefix = "wrap_access_token=";
            
            //if (!response.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            //{
            //    throw new ArgumentException("response");
            //}

            //var tokenString = response.Substring(prefix.Length);
            //return new SimpleWebToken(tokenString);

            var tokenString =
                HttpUtility.HtmlDecode(
                    Uri.UnescapeDataString(
                           response.Split('&')
                          .Single(value => value.StartsWith("wrap_access_token=", StringComparison.OrdinalIgnoreCase))
                          .Split('=')[1]));

            return new SimpleWebTokenHandler().ReadToken(tokenString) as SimpleWebToken;
        }

        private string RequestToken(NameValueCollection values)
        {
            using (var client = new WebClient())
            {
                client.BaseAddress = _issuerAddress.AbsoluteUri;

                byte[] responseBytes = client.UploadValues("", "POST", values);
                return Encoding.UTF8.GetString(responseBytes);
            }
        }
    }
}
