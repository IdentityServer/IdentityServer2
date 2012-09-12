using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Thinktecture.IdentityServer.Models.Configuration;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public class AdminConfigurationGeneralViewModel
    {
        public AdminConfigurationGeneralViewModel()
        {
        }
        public AdminConfigurationGeneralViewModel(GlobalConfiguration config)
        {
            this.DefaultHttpTokenType = config.DefaultHttpTokenType;
            this.DefaultTokenLifetime = config.DefaultTokenLifetime;
            this.DefaultWSTokenType = config.DefaultWSTokenType;
            this.EnableClientCertificateAuthentication = config.EnableClientCertificateAuthentication;
            this.EnforceUsersGroupMembership = config.EnforceUsersGroupMembership;
            this.HttpPort = config.HttpPort;
            this.HttpsPort = config.HttpsPort;
            this.IssuerContactEmail = config.IssuerContactEmail;
            this.IssuerUri = config.IssuerUri;
            this.MaximumTokenLifetime = config.MaximumTokenLifetime;
            this.RequireEncryption = config.RequireEncryption;
            this.RequireRelyingPartyRegistration = config.RequireRelyingPartyRegistration;
            this.SiteName = config.SiteName;
            this.SsoCookieLifetime = config.SsoCookieLifetime;
        }
        public GlobalConfiguration ToModel()
        {
            var config = new GlobalConfiguration();
            config.DefaultHttpTokenType = this.DefaultHttpTokenType;
            config.DefaultTokenLifetime = this.DefaultTokenLifetime;
            config.DefaultWSTokenType = this.DefaultWSTokenType;
            config.EnableClientCertificateAuthentication = this.EnableClientCertificateAuthentication;
            config.EnforceUsersGroupMembership = this.EnforceUsersGroupMembership;
            config.HttpPort = this.HttpPort;
            config.HttpsPort = this.HttpsPort;
            config.IssuerContactEmail = this.IssuerContactEmail;
            config.IssuerUri = this.IssuerUri;
            config.MaximumTokenLifetime = this.MaximumTokenLifetime;
            config.RequireEncryption = this.RequireEncryption;
            config.RequireRelyingPartyRegistration = this.RequireRelyingPartyRegistration;
            config.SiteName = this.SiteName;
            config.SsoCookieLifetime = this.SsoCookieLifetime;
            return config;
        }

        [Required]
        [Display(Name = "Site Name", Description = "Descriptive name of your STS.")]
        public String SiteName { get; set; }

        [Required]
        [Display(Name = "Site ID", Description = "Unique identifier for your STS. Must be a URI and will be the identifier issued in tokens and replying parties will use to identify your identity provider (this STS).")]
        public String IssuerUri { get; set; }

        [Display(Name = "Contact Email", Description = "Email of the contact person that administrates this STS.")]
        [RegularExpression("@", ErrorMessage = "{0} must be an email")]
        public String IssuerContactEmail { get; set; }

        [Display(Name = "Default WS Token", Description = "Default token type to be issues for WS-Federation and WS-Trust token requests.")]
        public string DefaultWSTokenType { get; set; }

        [Display(Name = "Default HTTP Token", Description = "Default token type to be issued for HTTP (non WS-*) token requests.")]
        public string DefaultHttpTokenType { get; set; }

        [Display(Name = "Default Token Lifetime", Description = "The default lifetime (in hours) of tokens issued from this STS.")]
        public int DefaultTokenLifetime { get; set; }

        [Display(Name = "Maximum Token Lifetime", Description = "The maximum lifetime (in hours) of tokens issued from this STS if a relying party requests a lifetime longer than the default.")]
        public int MaximumTokenLifetime { get; set; }

        [Display(Name = "SSO Duration", Description = "The duration (in hours) of the single sign-on cookies used for this web site.")]
        public int SsoCookieLifetime { get; set; }

        [Display(Name = "Require Encryption (SSL?)", Description = "When enabled this setting will require the client requesting tokens to be using a secure channel.")]
        public Boolean RequireEncryption { get; set; }

        [Display(Name = "Require RP Regitration", Description = "When enabled this will only issue tokens for relying parties that are registed with this STS.")]
        public Boolean RequireRelyingPartyRegistration { get; set; }

        [Display(Name = "Enable Client Certificates", Description = "Allow users to authenticate with client certificates.")]
        public Boolean EnableClientCertificateAuthentication { get; set; }

        [Display(Name = "Enforce Users in 'Users' Role", Description = "When enabled this setting will cause the STS to only issue tokens for users that are in the '" + Constants.Roles.IdentityServerUsers + "' role.")]
        public Boolean EnforceUsersGroupMembership { get; set; }

        [Display(Name = "HTTP Port", Description = "Port number the STS using for HTTP.")]
        public int HttpPort { get; set; }

        [Display(Name = "HTTPS Port", Description = "Port number the STS is using for HTTPS.")]
        public int HttpsPort { get; set; }

    }
}