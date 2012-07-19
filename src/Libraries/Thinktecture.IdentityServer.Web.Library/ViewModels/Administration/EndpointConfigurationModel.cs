/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Web.ViewModels.Administration
{
    public class EndpointConfigurationModel
    {
        [Required]
        [DisplayName("WS-Federation")]
        public Boolean WSFederation { get; set; }

        [Required]
        [DisplayName("WS-Federation Home Realm Discovery (Federation)")]
        public Boolean WSFederationHrd { get; set; }

        [Required]
        [DisplayName("WS-Trust Message Security")]
        public Boolean WSTrustMessage { get; set; }

        [Required]
        [DisplayName("WS-Trust MixedMode Security")]
        public Boolean WSTrustMixed { get; set; }

        [Required]
        [DisplayName("OAuth WRAP")]
        public Boolean OAuthWRAP { get; set; }

        [Required]
        [DisplayName("OAuth2")]
        public Boolean OAuth2 { get; set; }

        [Required]
        [DisplayName("JSNotify")]
        public Boolean JSNotify { get; set; }

        [Required]
        [DisplayName("Simple HTTP GET Endpoint")]
        public Boolean SimpleHttp { get; set; }

        [Required]
        [DisplayName("Federation Metadata")]
        public Boolean FederationMetadata { get; set; }

        [Required]
        [DisplayName("HTTP Port")]
        public int HttpPort { get; set; }

        [Required]
        [DisplayName("HTTPS Port")]
        public int HttpsPort { get; set; }
    }
}