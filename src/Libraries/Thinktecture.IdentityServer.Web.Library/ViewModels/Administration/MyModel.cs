/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.Security.Claims;

namespace Thinktecture.IdentityServer.Web.ViewModels.Administration
{
    public class MyModel
    {
        public List<Claim> Claims { get; set; }
        public string SamlToken { get; set; }
    }
}