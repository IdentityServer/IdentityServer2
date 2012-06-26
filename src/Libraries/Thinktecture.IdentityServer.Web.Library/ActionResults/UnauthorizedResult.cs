/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Web.ActionResults
{
    public class UnauthorizedResult : ActionResult
    {
        int _statusCode = 401;
        string _scheme;
        ResponseAction _responseAction = ResponseAction.Send401;

        public enum ResponseAction
        {
            Send401,
            RedirectToLoginPage
        }

        public UnauthorizedResult(string scheme)
            : this (scheme, ResponseAction.Send401)
        { }

        public UnauthorizedResult(string scheme, ResponseAction responseAction)
        {
            _scheme = scheme;
            _responseAction = responseAction;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.HttpContext.Response.StatusCode = _statusCode;
            
            if (!string.IsNullOrWhiteSpace(_scheme))
            {
                context.HttpContext.Response.Headers.Add("WWW-Authenticate", _scheme);
            }

            if (_responseAction == ResponseAction.Send401)
            {
                context.HttpContext.Items["NoRedirect"] = true;
            }
        }
    }
}