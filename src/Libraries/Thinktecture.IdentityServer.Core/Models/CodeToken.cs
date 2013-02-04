/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models
{
    public class CodeToken
    {
        public string Code { get; set; }

        public int ClientId { get; set; }

        public string UserName { get; set; }

        public string Scope { get; set; }

        public CodeTokenType Type { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
