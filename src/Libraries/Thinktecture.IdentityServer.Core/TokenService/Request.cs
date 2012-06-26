/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.Composition;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.TokenService
{
    /// <summary>
    /// This class analyzes the token request so the STS can inspect the information later
    /// </summary>
    public class Request
    {
        GlobalConfiguration _configuration;
        RequestDetails _details;

        [Import]
        public IRelyingPartyRepository RelyingPartyRepository { get; set; }

        [Import]
        public IDelegationRepository DelegationRepository { get; set; }

        public Request(GlobalConfiguration configuration)
        {
            _configuration = configuration;
            Container.Current.SatisfyImportsOnce(this);
        }

        public Request(GlobalConfiguration configuration, IRelyingPartyRepository relyingPartyRepository, IDelegationRepository delegationRepository)
        {
            _configuration = configuration;
            RelyingPartyRepository = relyingPartyRepository;
            DelegationRepository = delegationRepository;
        }

        public RequestDetails Analyze(RequestSecurityToken rst, ClaimsPrincipal principal)
        {
            if (rst == null)
            {
                throw new ArgumentNullException("rst");
            }

            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            Tracing.Information("Starting PolicyOptions creation");

            var clientIdentity = AnalyzeClientIdentity(principal);

            var details = new RequestDetails
            {
                ClientIdentity = clientIdentity,
                IsActive = false,
                Realm = null,
                IsKnownRealm = false,
                UsesSsl = false,
                UsesEncryption = false,
                ReplyToAddress = null,
                ReplyToAddressIsWithinRealm = false,
                IsReplyToFromConfiguration = false,
                EncryptingCertificate = null,
                ClaimsRequested = false,
                RequestClaims = null,
                Request = null,
                IsActAsRequest = false,
                RelyingPartyRegistration = null
            };

            AnalyzeRst(rst, details);
            AnalyzeTokenType(rst, details);
            AnalyzeKeyType(rst);
            AnalyzeRealm(rst, details);
            AnalyzeOperationContext(details);
            AnalyzeDelegation(rst, details);
            AnalyzeRelyingParty(details);
            AnalyzeEncryption(details);
            AnalyzeReplyTo(details);
            AnalyzeSsl(details);
            AnalyzeRequestClaims(details);

            Tracing.Information("PolicyOptions creation done.");

            _details = details;
            return details;
        }

        public void Validate()
        {
            Validate(_details);
        }

        public void Validate(RequestDetails details)
        {
            if (details == null)
            {
                throw new ArgumentNullException("details");
            }

            Tracing.Information("Starting policy validation");

            ValidateKnownRealm(details);
            ValidateTokenType(details);
            ValidateReplyTo(details);
            ValidateEncryption(details);
            ValidateSsl(details);
            ValidateDelegation(details);

            Tracing.Information("Policy Validation succeeded");
        }
        
        #region Analyze
        protected virtual void AnalyzeRequestClaims(RequestDetails details)
        {
            // check if specific claims are requested
            if (details.Request.Claims != null && details.Request.Claims.Count > 0)
            {
                details.ClaimsRequested = true;
                details.RequestClaims = details.Request.Claims;

                var requestClaims = new StringBuilder(20);
                details.RequestClaims.ToList().ForEach(rq => requestClaims.AppendFormat("{0}\n", rq.ClaimType));
                Tracing.Information("Specific claims requested");
                Tracing.Information(String.Format("Request claims: {0}", requestClaims));
            }
            else
            {
                Tracing.Information("No request claims");
            }
        }

        protected virtual void AnalyzeSsl(RequestDetails details)
        {
            // determine if reply to is via SSL
            details.UsesSsl = (details.ReplyToAddress.Scheme == Uri.UriSchemeHttps);
            Tracing.Information(String.Format("SSL used:{0}", details.UsesSsl));
        }

        protected virtual void AnalyzeReplyTo(RequestDetails details)
        {
            var rp = details.RelyingPartyRegistration;

            // determine the reply to address (only relevant for passive requests)
            if (rp != null && rp.ReplyTo != null)
            {
                details.ReplyToAddress = rp.ReplyTo;
                details.IsReplyToFromConfiguration = true;

                // check if reply to is a sub-address of the realm address
                if (details.ReplyToAddress.AbsoluteUri.StartsWith(details.Realm.Uri.AbsoluteUri, StringComparison.OrdinalIgnoreCase))
                {
                    details.ReplyToAddressIsWithinRealm = true;
                }

                Tracing.Information(String.Format("ReplyTo Address set from configuration: {0}", details.ReplyToAddress.AbsoluteUri));
            }
            else
            {
                if (!String.IsNullOrEmpty(details.Request.ReplyTo))
                {
                    if (_configuration.AllowReplyTo)
                    {
                        // explicit address
                        details.ReplyToAddress = new Uri(details.Request.ReplyTo);
                        Tracing.Information(String.Format("Explicit ReplyTo address set: {0}", details.ReplyToAddress.AbsoluteUri));

                        // check if reply to is a sub-address of the realm address
                        if (details.ReplyToAddress.AbsoluteUri.StartsWith(details.Realm.Uri.AbsoluteUri, StringComparison.OrdinalIgnoreCase))
                        {
                            details.ReplyToAddressIsWithinRealm = true;
                        }

                        Tracing.Information(String.Format("ReplyTo Address is within Realm: {0}", details.ReplyToAddressIsWithinRealm));
                    }
                    else
                    {
                        // same as realm
                        details.ReplyToAddress = details.Realm.Uri;
                        details.ReplyToAddressIsWithinRealm = true;
                        Tracing.Warning(string.Format("ReplyTo address of ({0}) was supplied, but since configuration does not allow ReplyTo, the realm address is used", details.Request.ReplyTo));
                    }
                }
                else
                {
                    // same as realm
                    details.ReplyToAddress = details.Realm.Uri;
                    details.ReplyToAddressIsWithinRealm = true;
                    Tracing.Information("ReplyTo address set to realm address");
                }
            }
        }

        protected virtual void AnalyzeTokenType(RequestSecurityToken rst, RequestDetails details)
        {
            if (string.IsNullOrWhiteSpace(rst.TokenType))
            {
                details.TokenType = _configuration.DefaultTokenType;
                Tracing.Information("Token Type: not specified, falling back to default token type");
            }
            else
            {
                Tracing.Information("Token Type: " + rst.TokenType);
                details.TokenType = rst.TokenType;
            }
        }

        protected virtual void AnalyzeEncryption(RequestDetails details)
        {
            if (details.EncryptingCertificate == null)
            {
                X509Certificate2 requestCertificate;
                if (TryGetEncryptionCertificateFromRequest(details.Realm, out requestCertificate))
                {
                    details.EncryptingCertificate = requestCertificate;
                    Tracing.Information("Encrypting certificate set from RST");
                }
            }

            details.UsesEncryption = (details.EncryptingCertificate != null);
            Tracing.Information("Token encryption: " + details.UsesEncryption);
        }

        protected virtual RelyingParty AnalyzeRelyingParty(RequestDetails details)
        {
            // check if the relying party is registered
            RelyingParty rp = null;
            if (RelyingPartyRepository.TryGet(details.Realm.Uri.AbsoluteUri, out rp))
            {
                details.RelyingPartyRegistration = rp;
                details.IsKnownRealm = true;

                var traceString = String.Format("Relying Party found in registry - Realm: {0}", rp.Realm.AbsoluteUri);

                if (!string.IsNullOrEmpty(rp.Name))
                {
                    traceString += String.Format(" ({0})", rp.Name);
                }

                Tracing.Information(traceString);

                if (rp.EncryptingCertificate != null)
                {
                    details.EncryptingCertificate = rp.EncryptingCertificate;
                    Tracing.Information("Encrypting certificate set from registry");
                }
            }
            else
            {
                Tracing.Information("Relying party is not registered.");
            }
            return rp;
        }

        protected virtual void AnalyzeDelegation(RequestSecurityToken rst, RequestDetails details)
        {
            // check for identity delegation request
            if (rst.ActAs != null)
            {
                details.IsActAsRequest = true;
                Tracing.Information("Request is ActAs request");
            }
        }

        protected virtual void AnalyzeKeyType(RequestSecurityToken rst)
        {
            if (!string.IsNullOrEmpty(rst.KeyType))
            {
                Tracing.Information(String.Format("Requested KeyType: {0}", rst.KeyType));
            }
        }

        protected virtual void AnalyzeOperationContext(RequestDetails details)
        {
            // determine if this is a WCF call
            if (OperationContext.Current != null)
            {
                details.IsActive = true;
                Tracing.Information("Active request");
            }
            else
            {
                Tracing.Information("Passive request");
            }
        }

        protected virtual ClaimsIdentity AnalyzeClientIdentity(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (principal.Identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            var clientIdentity = principal.Identity as ClaimsIdentity;

            if (!clientIdentity.IsAuthenticated)
            {
                Tracing.Error("Client Identity is anonymous");
                throw new ArgumentException("client identity");
            }

            return clientIdentity;
        }

        protected virtual void AnalyzeRst(RequestSecurityToken rst, RequestDetails options)
        {
            if (rst == null)
            {
                throw new ArgumentNullException("request");
            }

            options.Request = rst;
        }

        protected virtual void AnalyzeRealm(RequestSecurityToken rst, RequestDetails options)
        {
            // check realm
            if (rst.AppliesTo == null || rst.AppliesTo.Uri == null)
            {
                throw new Exception("AppliesTo is missing");
                //throw new MissingAppliesToException("AppliesTo is missing");
            }

            options.Realm = new EndpointAddress(rst.AppliesTo.Uri);
        }
        #endregion

        #region Validate
        protected virtual void ValidateDelegation(RequestDetails details)
        {
            // check for ActAs request
            if (details.IsActAsRequest)
            {
                if (!_configuration.EnableDelegation)
                {
                    Tracing.Error("Request is ActAs request - but ActAs is not enabled");
                    throw new InvalidRequestException("Request is ActAs request - but ActAs is not enabled");
                }

                if (!DelegationRepository.IsDelegationAllowed(details.ClientIdentity.Name, details.Realm.Uri.AbsoluteUri))
                {
                    Tracing.Error(String.Format("ActAs mapping not found."));
                    throw new InvalidRequestException("ActAs mapping not found.");
                }
            }
        }

        protected virtual void ValidateSsl(RequestDetails details)
        {
            // check if SSL is used (for passive only)
            if (_configuration.RequireSsl && !details.UsesSsl)
            {
                if (!details.IsActive)
                {
                    Tracing.Error("Configuration requires SSL - but clear text reply address used");
                    throw new InvalidRequestException("SSL is required");
                }
            }
        }

        private void ValidateTokenType(RequestDetails details)
        {
            if (details.TokenType == TokenTypes.SimpleWebToken)
            {
                if (details.RelyingPartyRegistration == null ||
                    details.RelyingPartyRegistration.SymmetricSigningKey == null ||
                    details.RelyingPartyRegistration.SymmetricSigningKey.Length == 0)
                {
                    Tracing.Error("SWT token requested, but no symmetric signing key found");
                    throw new InvalidRequestException("SWT token requested, but no symmetric signing key found");
                }
            }
        }

        protected virtual void ValidateEncryption(RequestDetails details)
        {
            // check if token must be encrypted
            if (_configuration.RequireEncryption && (!details.UsesEncryption))
            {
                Tracing.Error("Configuration requires encryption - but no key available");
                throw new InvalidRequestException("No encryption key available");
            }
        }

        protected virtual void ValidateReplyTo(RequestDetails details)
        {
            // check if replyto is part of a registered realm (when not explicitly registered in config)
            if (!details.IsReplyToFromConfiguration)
            {
                if (_configuration.RequireReplyToWithinRealm && (!details.ReplyToAddressIsWithinRealm))
                {
                    Tracing.Error("Configuration requires that ReplyTo is a sub-address of the realm - this is not the case");
                    throw new InvalidRequestException("Invalid ReplyTo");
                }
            }
        }

        protected virtual void ValidateKnownRealm(RequestDetails details)
        {
            // check if realm is allowed
            if (_configuration.AllowKnownRealmsOnly && (!details.IsKnownRealm))
            {
                Tracing.Error("Configuration requires a known realm - but realm is not registered");

                throw new Exception(details.Realm.Uri.AbsoluteUri);
                //throw new InvalidScopeException(details.Realm.Uri.AbsoluteUri);
            }
        }
        #endregion

        protected virtual bool TryGetEncryptionCertificateFromRequest(EndpointAddress appliesTo, out X509Certificate2 certificate)
        {
            if (appliesTo == null)
            {
                throw new ArgumentNullException("appliesTo");
            }
            
            certificate = null;

            var epi = appliesTo.Identity as X509CertificateEndpointIdentity;
            if (epi != null && epi.Certificates.Count > 0)
            {
                certificate = epi.GetEndCertificate();
                return true;
            }

            // no cert found
            return false;
        }
    }
}