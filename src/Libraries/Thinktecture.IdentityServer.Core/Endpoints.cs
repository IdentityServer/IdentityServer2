/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;

namespace Thinktecture.IdentityServer
{
    /// <summary>
    /// Creates endpoint URIs for the various STS endpoints
    /// </summary>
    public class Endpoints
    {
        public Uri WSFederation { get; set; }
        public Uri WSFederationHRD { get; set; }
        public Uri WSFederationMetadata { get; set; }
        public Uri WSTrustMex { get; set; }
        public Uri PrivacyNotice { get; set; }

        public Uri WSTrustMessageUserName { get; set; }
        public Uri WSTrustMixedUserName { get; set; }
        public Uri WSTrustMessageCertificate { get; set; }
        public Uri WSTrustMixedCertificate { get; set; }
        
        public Uri SimpleHttp { get; set; }
        public Uri Wrap { get; set; }
        public Uri OAuth2 { get; set; }
        public Uri JSNotify { get; set; }

        public static class Paths
        {
            public const string WSFedIssuePage = "/issue/wsfed";
            public const string WSFedHRD = "/issue/hrd";
            public const string WSFedMetadata = "/FederationMetadata/2007-06/FederationMetadata.xml";
            public const string PrivacyNotice = "/privacyNotice.txt";
            public const string WSTrustBase = "/issue/wstrust";
            public const string SimpleHttp = "/issue/simple";
            public const string Wrap = "/issue/wrap";
            public const string OAuth2 = "/issue/oauth2";
            public const string JSNotify = "/issue/jsnotify";
            public const string Mex = "/mex";
            public const string WSTrustMessageUserName = "/message/username";
            public const string WSTrustMixedUserName = "/mixed/username";
            public const string WSTrustMessageCertificate = "/message/certificate";
            public const string WSTrustMixedCertificate = "/mixed/certificate";
        }

        /// <summary>
        /// Creates the URIs based on the host header
        /// </summary>
        /// <returns>STS Endpoints</returns>
        public static Endpoints Create(string host, string applicationPath, int httpPort, int httpsPort)
        {
            return Create("https://" + host + applicationPath, httpPort, httpsPort);
        }

        /// <summary>
        /// Creates the URIs based on the baseUri.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <returns>STS Endpoints</returns>
        public static Endpoints Create(string baseUriString, int httpPort, int httpsPort)
        {
            var ep = new Endpoints();
            if (baseUriString.EndsWith("/"))
            {
                baseUriString = baseUriString.Substring(0, baseUriString.Length - 1);
            }
            
            // construct various http and https URIs
            var passive = new Uri(baseUriString + Paths.WSFedIssuePage);
            var builder = new UriBuilder(passive);
            builder.Scheme = Uri.UriSchemeHttps;
            builder.Port = httpsPort;
            ep.WSFederation = builder.Uri;

            var hrd = new Uri(baseUriString + Paths.WSFedHRD);
            builder = new UriBuilder(hrd);
            builder.Scheme = Uri.UriSchemeHttps;
            builder.Port = httpsPort;
            ep.WSFederationHRD = builder.Uri;

            // construct various http and https URIs
            var privacy = new Uri(baseUriString + Paths.PrivacyNotice);
            builder = new UriBuilder(privacy);
            builder.Scheme = Uri.UriSchemeHttps;
            builder.Port = httpsPort;
            ep.PrivacyNotice = builder.Uri;

            var simpleHttp = new Uri(baseUriString + Paths.SimpleHttp);
            builder = new UriBuilder(simpleHttp);
            builder.Scheme = Uri.UriSchemeHttps;
            builder.Port = httpsPort;
            ep.SimpleHttp = builder.Uri;

            var wrap = new Uri(baseUriString + Paths.Wrap);
            builder = new UriBuilder(wrap);
            builder.Scheme = Uri.UriSchemeHttps;
            builder.Port = httpsPort;
            ep.Wrap = builder.Uri;

            var oauth2 = new Uri(baseUriString + Paths.OAuth2);
            builder = new UriBuilder(oauth2);
            builder.Scheme = Uri.UriSchemeHttps;
            builder.Port = httpsPort;
            ep.OAuth2 = builder.Uri;

            var jsnotify = new Uri(baseUriString + Paths.JSNotify);
            builder = new UriBuilder(jsnotify);
            builder.Scheme = Uri.UriSchemeHttps;
            builder.Port = httpsPort;
            ep.JSNotify = builder.Uri;

            var wsfedmd = new Uri(baseUriString + Paths.WSFedMetadata);
            builder = new UriBuilder(wsfedmd);
            builder.Scheme = Uri.UriSchemeHttps;
            builder.Port = httpsPort;
            ep.WSFederationMetadata = builder.Uri;

            var activeClear = new Uri(baseUriString + Paths.WSTrustBase);
            builder = new UriBuilder(activeClear);
            builder.Scheme = Uri.UriSchemeHttp;
            builder.Port = httpPort;
            activeClear = builder.Uri;

            builder = new UriBuilder(activeClear);
            builder.Scheme = Uri.UriSchemeHttps;
            builder.Port = httpsPort;
            var activeSsl = builder.Uri;

            ep.WSTrustMessageUserName = new Uri(activeClear + Paths.WSTrustMessageUserName);
            ep.WSTrustMixedUserName = new Uri(activeSsl + Paths.WSTrustMixedUserName);

            ep.WSTrustMessageCertificate = new Uri(activeClear + Paths.WSTrustMessageCertificate);
            ep.WSTrustMixedCertificate = new Uri(activeSsl + Paths.WSTrustMixedCertificate);

            ep.WSTrustMex = new Uri(activeSsl + Paths.Mex);

            return ep;
        }
    }
}