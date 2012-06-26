/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;

namespace Thinktecture.IdentityServer.Models
{
    public class GlobalConfiguration
    {
        public String SiteName { get; set; }
        public String IssuerUri { get; set; }
        public String IssuerContactEmail { get; set; }
        public string DefaultTokenType { get; set; }
        public int DefaultTokenLifetime { get; set; }
        public int MaximumTokenLifetime { get; set; }
        public int SsoCookieLifetime { get; set; }
        public Boolean RequireSsl { get; set; }
        public Boolean RequireEncryption { get; set; }
        public Boolean RequireSignInConfirmation { get; set; }
        public Boolean RequireReplyToWithinRealm { get; set; }
        public Boolean AllowKnownRealmsOnly { get; set; }
        public Boolean AllowReplyTo { get; set; }
        public Boolean EnableClientCertificates { get; set; }
        public Boolean EnableDelegation { get; set; }
        public Boolean EnableStrongEpiForSsl { get; set; }
        public Boolean EnableFederationMessageTracing { get; set; }
        public Boolean EnforceUsersGroupMembership { get; set; }
    }
}


