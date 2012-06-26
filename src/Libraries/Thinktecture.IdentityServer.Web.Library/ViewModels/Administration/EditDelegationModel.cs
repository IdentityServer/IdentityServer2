/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Web.ViewModels.Administration
{
    public class EditDelegationModel
    {
        public string UserName { get; set; }
        public IEnumerable<DelegationSetting> Settings { get; set; }
    }
}