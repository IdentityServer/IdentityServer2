/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.IdentityModel.Services;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Protocols.WSFederation
{
    public class WSFederationResult : ContentResult
    {
        public WSFederationResult(SignInResponseMessage message, bool requireSsl)
        {
            if (requireSsl)
            {
                if (message.BaseUri.Scheme != Uri.UriSchemeHttps)
                {
                    throw new InvalidOperationException("Return URL must be SSL.");
                }
            }

            Content = message.WriteFormPost();
        }
    }
}