/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Web.ViewModels.Administration
{
    public class GlobalConfigurationModel
    {
        [Required]
        [DisplayName("Site Name")]
        public String SiteName { get; set; }

        [Required]
        [DisplayName("Issuer URI")]
        public String IssuerUri { get; set; }

        [Required]
        [DisplayName("Issuer Contact Email")]
        public String IssuerContactEmail { get; set; }

        [Required]
        [DisplayName("Default Token Type")]
        public string DefaultTokenType { get; set; }

        [Required]
        [DisplayName("Default Token Lifetime (in hours)")]
        public int DefaultTokenLifetime { get; set; }

        [Required]
        [DisplayName("Maximum Token Lifetime (in hours)")]
        public int MaximumTokenLifetime { get; set; }

        [Required]
        [DisplayName("Single Sign-On Cookie Lifetime (in hours)")]
        public int SsoCookieLifetime { get; set; }

        [Required]
        [DisplayName("Require SSL")]
        public Boolean RequireSsl { get; set; }

        [Required]
        [DisplayName("Require Encryption")]
        public Boolean RequireEncryption { get; set; }

        [Required]
        [DisplayName("Enable Client Certificates")]
        public Boolean EnableClientCertificates { get; set; }

        [Required]
        [DisplayName("Enable Identity Delegation")]
        public Boolean EnableDelegation { get; set; }

        [Required]
        [DisplayName("Only allow known Realms")]
        public Boolean AllowKnownRealmsOnly { get; set; }

        [Required]
        [DisplayName("Allow Reply To Parameter")]
        public Boolean AllowReplyTo { get; set; }

        [Required]
        [DisplayName("Require Reply To within Realm")]
        public Boolean RequireReplyToWithinRealm { get; set; }

        [Required]
        [DisplayName("Enable Strong Endpoint Identities for WS-Trust SSL Endpoints")]
        public Boolean EnableStrongEpiForSsl { get; set; }

        [Required]
        [DisplayName("Enable Tracing of Federation Messages (RST, RSTR, issued token)")]
        public Boolean EnableFederationMessageTracing { get; set; }

        [Required]
        [DisplayName("Enforce 'IdentityServerUsers' role for clients")]
        public Boolean EnforceUsersGroupMembership { get; set; }

        [Required]
        [DisplayName("Require Sign In Confirmation")]
        public Boolean RequireSignInConfirmation { get; set; }
    }
}