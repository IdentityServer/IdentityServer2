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
using System.Json;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace Thinktecture.IdentityServer.OAuth
{
    public class OAuth2Client
    {
        HttpClient _client;

        public OAuth2Client(Uri address)
        {
            _client = new HttpClient { BaseAddress = address };
        }

        public AccessTokenResponse RequestAccessTokenCertificate(X509Certificate2 certificate, string scope)
        {
            var handler = new WebRequestHandler();
            handler.ClientCertificates.Add(certificate);

            var client = new HttpClient(handler) { BaseAddress = _client.BaseAddress };
            var response = client.PostAsync("", CreateFormCertificate(scope)).Result;
            response.EnsureSuccessStatusCode();

            var json = JsonValue.Parse(response.Content.ReadAsStringAsync().Result).AsDynamic();
            return CreateResponseFromJson(json);
        }

        public AccessTokenResponse RequestAccessTokenUserName(string userName, string password, string scope)
        {
            var response = _client.PostAsync("", CreateFormUserName(userName, password, scope)).Result;
            response.EnsureSuccessStatusCode();

            var json = JsonValue.Parse(response.Content.ReadAsStringAsync().Result).AsDynamic();
            return CreateResponseFromJson(json);
        }

        public AccessTokenResponse RequestAccessTokenAssertion(string assertion, string assertionType, string scope)
        {
            var response = _client.PostAsync("", CreateFormAssertion(assertion, assertionType, scope)).Result;
            response.EnsureSuccessStatusCode();

            var json = JsonValue.Parse(response.Content.ReadAsStringAsync().Result).AsDynamic();
            return CreateResponseFromJson(json);
        }

        protected virtual FormUrlEncodedContent CreateFormCertificate(string scope)
        {
            var values = new Dictionary<string, string>
            {
                { OAuth2Constants.GrantType, OAuth2Constants.ClientCredentials },
                { OAuth2Constants.scope, scope }
            };

            return new FormUrlEncodedContent(values);
        }

        protected virtual FormUrlEncodedContent CreateFormUserName(string userName, string password, string scope)
        {
            var values = new Dictionary<string, string>
            {
                { OAuth2Constants.GrantType, OAuth2Constants.Password },
                { OAuth2Constants.UserName, userName },
                { OAuth2Constants.Password, password },
                { OAuth2Constants.scope, scope }
            };

            return new FormUrlEncodedContent(values);
        }

        protected virtual FormUrlEncodedContent CreateFormAssertion(string assertion, string assertionType, string scope)
        {
            var values = new Dictionary<string, string>
            {
                { OAuth2Constants.GrantType, assertionType },
                { OAuth2Constants.Assertion, assertion },
                { OAuth2Constants.scope, scope }
            };

            return new FormUrlEncodedContent(values);
        }

        private AccessTokenResponse CreateResponseFromJson(dynamic json)
        {
            return new AccessTokenResponse
            {
                AccessToken = json.access_token,
                RefreshToken = json.refresh_token,
                TokenType = json.token_type,
                ExpiresIn = json.expires_in
            };
        }

    } 
}
