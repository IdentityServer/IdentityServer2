/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Web;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Protocols
{
    public class WrapResult : ActionResult
    {
        public TokenResponse TokenResponse { get; set; }

        protected string _contentType = "text/plain";
        protected string _content;

        public WrapResult()
        { }

        public WrapResult(TokenResponse response)
        {
            TokenResponse = response;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _content = "wrap_access_token=" + Uri.EscapeDataString(TokenResponse.TokenString);

            WriteToken(context);
        }

        protected virtual void WriteToken(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();

            response.ContentType = _contentType;
            response.Write(_content);
        }
    }
}
