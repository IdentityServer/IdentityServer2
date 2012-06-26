/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class Global
    {
        [Key]
        public string Name { get; set; }

        [Required]
        public String SiteName { get; set; }

        [Required]
        public String IssuerUri { get; set; }

        [Required]
        public String IssuerContactEmail { get; set; }

        [Required]
        public string DefaultTokenType { get; set; }

        [Required]
        public int DefaultTokenLifetime { get; set; }

        [Required]
        public int MaximumTokenLifetime { get; set; }

        [Required]
        public int SsoCookieLifetime { get; set; }

        [Required]
        public Boolean RequireSsl { get; set; }

        [Required]
        public Boolean RequireEncryption { get; set; }

        [Required]
        public Boolean RequireSignInConfirmation { get; set; }

        [Required]
        public Boolean RequireReplyToWithinRealm { get; set; }

        [Required]
        public Boolean AllowKnownRealmsOnly { get; set; }

        [Required]
        public Boolean AllowReplyTo { get; set; }

        [Required]
        public Boolean EnableClientCertificates { get; set; }

        [Required]
        public Boolean EnableDelegation { get; set; }

        [Required]
        public Boolean EnableStrongEpiForSsl { get; set; }

        [Required]
        public Boolean EnableFederationMessageTracing { get; set; }

        [Required]
        public Boolean EnforceUsersGroupMembership { get; set; }
    }
}
