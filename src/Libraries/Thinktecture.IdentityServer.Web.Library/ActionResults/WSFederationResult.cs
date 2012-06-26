/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.IdentityModel.Services;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Web.ActionResults
{
    public class WSFederationResult : ContentResult
    {
        public WSFederationResult(SignInResponseMessage message)
        {
            Content = message.WriteFormPost();
        }
    }
}