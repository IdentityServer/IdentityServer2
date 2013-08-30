/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Web.GlobalFilter
{
    public class SslRedirectFilter : ActionFilterAttribute
    {
        int _port = 443;
        string _host;

        public SslRedirectFilter(int sslPort, string host)
        {
            _port = sslPort;
            _host = host;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsSecureConnection)
            {
                var url = GetAbsoluteUri(filterContext.HttpContext.Request.Url).AbsoluteUri;
                filterContext.Result = new RedirectResult(url, true);
            }
        }

        private Uri GetAbsoluteUri(Uri uriFromCaller)
        {
            var host = String.IsNullOrWhiteSpace(_host) ? uriFromCaller.Host : _host;
            UriBuilder builder = new UriBuilder(Uri.UriSchemeHttps, host);
            builder.Path = uriFromCaller.GetComponents(UriComponents.Path, UriFormat.Unescaped);
            builder.Port = _port;

            string query = uriFromCaller.GetComponents(UriComponents.Query, UriFormat.UriEscaped);
            if (query.Length > 0)
            {
                string uriWithoutQuery = builder.Uri.AbsoluteUri;
                string absoluteUri = string.Format("{0}?{1}", uriWithoutQuery, query);
                return new Uri(absoluteUri, UriKind.Absolute);
            }
            else
            {
                return builder.Uri;
            }
        }
    }
}