/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.ComponentModel.Composition;
using System.IdentityModel.Configuration;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.TokenService
{
    /// <summary>
    /// Configuration information for the token service
    /// </summary>
    public class TokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        private static readonly object _syncRoot = new object();
        private static Lazy<TokenServiceConfiguration> _configuration = new Lazy<TokenServiceConfiguration>();

        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public TokenServiceConfiguration() : base()
        {
            Tracing.Information("Configuring token service");
            Container.Current.SatisfyImportsOnce(this);

            SecurityTokenService = typeof(TokenService);
            DefaultTokenLifetime = TimeSpan.FromHours(ConfigurationRepository.Global.DefaultTokenLifetime);
            MaximumTokenLifetime = TimeSpan.FromDays(ConfigurationRepository.Global.MaximumTokenLifetime);
            DefaultTokenType = ConfigurationRepository.Global.DefaultWSTokenType;

            TokenIssuerName = ConfigurationRepository.Global.IssuerUri;
            SigningCredentials = new X509SigningCredentials(ConfigurationRepository.Keys.SigningCertificate);

            if (ConfigurationRepository.WSTrust.EnableDelegation)
            {
                Tracing.Information("Configuring identity delegation support");

                try
                {
                    var actAsRegistry = new ConfigurationBasedIssuerNameRegistry();
                    actAsRegistry.AddTrustedIssuer(ConfigurationRepository.Keys.SigningCertificate.Thumbprint, ConfigurationRepository.Global.IssuerUri);

                    var actAsHandlers = SecurityTokenHandlerCollectionManager["ActAs"];
                    actAsHandlers.Configuration.IssuerNameRegistry = actAsRegistry;
                    actAsHandlers.Configuration.AudienceRestriction.AudienceMode = AudienceUriMode.Never;
                }
                catch (Exception ex)
                {
                    Tracing.Error("Error configuring identity delegation");
                    Tracing.Error(ex.ToString());
                    throw;
                }
            }
        }

        public static TokenServiceConfiguration Current
        {
            get
            {
                return _configuration.Value;
            }
        }
    }
}