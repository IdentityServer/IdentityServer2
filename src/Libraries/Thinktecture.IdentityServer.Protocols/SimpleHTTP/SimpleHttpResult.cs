/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.IdentityModel.Tokens;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Constants;

namespace Thinktecture.IdentityServer.Protocols.SimpleHttp
{
    public class SimpleHttpResult : ContentResult
    {
        public SimpleHttpResult(string token, string tokenType)
        {
            Content = token;

            if (tokenType == TokenTypes.OasisWssSaml11TokenProfile11 ||
                tokenType == TokenTypes.Saml11TokenProfile11 ||
                tokenType == TokenTypes.OasisWssSaml2TokenProfile11 ||
                tokenType == TokenTypes.Saml2TokenProfile11)
            {
                ContentType = "text/xml";
            }
            else
            {
                ContentType = "text/plain";
            }
        }

        protected virtual void WriteToken(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();

            base.ExecuteResult(context);
        }
    }
}