/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.IdentityModel.Services;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Protocols.WSFederation
{
    public class WSFederationResult : ContentResult
    {
        public WSFederationResult(SignInResponseMessage message)
        {
            Content = message.WriteFormPost();
        }
    }
}