/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class DiagnosticsConfiguration
    {
        [Display(Name = "Enable tracing of federation message", Description = "Enable tracing of federation message (may contain confidential data).")]
        public Boolean EnableFederationMessageTracing { get; set; }
    }
}
