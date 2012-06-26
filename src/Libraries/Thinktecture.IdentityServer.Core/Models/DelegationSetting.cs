/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;

namespace Thinktecture.IdentityServer.Models
{
    public class DelegationSetting
    {
        public string UserName { get; set; }
        public Uri Realm { get; set; }
        public string Description { get; set; }
    }
}
