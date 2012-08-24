/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityServer.Models;
using Entities = Thinktecture.IdentityServer.Repositories.Sql;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    internal static class Mappings
    {
        #region GlobalConfiguration
        public static Models.Configuration.GlobalConfiguration ToDomainModel(this Entities.Configuration.GlobalConfiguration entity)
        {
            return new Models.Configuration.GlobalConfiguration
            {
                SiteName = entity.SiteName,
                IssuerUri = entity.IssuerUri,
                IssuerContactEmail = entity.IssuerContactEmail,
                DefaultHttpTokenType = entity.DefaultHttpTokenType,
                DefaultWSTokenType = entity.DefaultWSTokenType,
                DefaultTokenLifetime = entity.DefaultTokenLifetime,
                MaximumTokenLifetime = entity.MaximumTokenLifetime,
                EnableClientCertificateAuthentication = entity.EnableClientCertificateAuthentication,
                EnforceUsersGroupMembership = entity.EnforceUsersGroupMembership,
                HttpPort = entity.HttpPort,
                HttpsPort = entity.HttpsPort,
                RequireEncryption = entity.RequireEncryption,
                RequireRelyingPartyRegistration = entity.RequireRelyingPartyRegistration,
                SsoCookieLifetime = entity.SsoCookieLifetime
            };
        }

        public static Entities.Configuration.GlobalConfiguration ToEntity(this Models.Configuration.GlobalConfiguration model)
        {
            return new Entities.Configuration.GlobalConfiguration
            {
                SiteName = model.SiteName,
                IssuerUri = model.IssuerUri,
                IssuerContactEmail = model.IssuerContactEmail,
                DefaultHttpTokenType = model.DefaultHttpTokenType,
                DefaultWSTokenType = model.DefaultWSTokenType,
                DefaultTokenLifetime = model.DefaultTokenLifetime,
                MaximumTokenLifetime = model.MaximumTokenLifetime,
                EnableClientCertificateAuthentication = model.EnableClientCertificateAuthentication,
                EnforceUsersGroupMembership = model.EnforceUsersGroupMembership,
                HttpPort = model.HttpPort,
                HttpsPort = model.HttpsPort,
                RequireEncryption = model.RequireEncryption,
                RequireRelyingPartyRegistration = model.RequireRelyingPartyRegistration,
                SsoCookieLifetime = model.SsoCookieLifetime
            };
        }
        #endregion

        #region WSFederationConfiguration
        public static Models.Configuration.WSFederationConfiguration ToDomainModel(this Entities.Configuration.WSFederationConfiguration entity)
        {
            return new Models.Configuration.WSFederationConfiguration
            {
                AllowReplyTo = entity.AllowReplyTo,
                EnableAuthentication = entity.EnableAuthentication,
                Enabled = entity.Enabled,
                EnableFederation = entity.EnableFederation,
                EnableHrd = entity.EnableHrd,
                RequireReplyToWithinRealm = entity.RequireReplyToWithinRealm,
                RequireSslForReplyTo = entity.RequireSslForReplyTo
            };
        }

        public static Entities.Configuration.WSFederationConfiguration ToEntity(this Models.Configuration.WSFederationConfiguration model)
        {
            return new Entities.Configuration.WSFederationConfiguration
            {
                AllowReplyTo = model.AllowReplyTo,
                EnableAuthentication = model.EnableAuthentication,
                Enabled = model.Enabled,
                EnableFederation = model.EnableFederation,
                EnableHrd = model.EnableHrd,
                RequireReplyToWithinRealm = model.RequireReplyToWithinRealm,
                RequireSslForReplyTo = model.RequireSslForReplyTo
            };
        }
        #endregion

        #region KeyMaterialConfiguration
        public static Models.Configuration.KeyMaterialConfiguration ToDomainModel(this Entities.Configuration.KeyMaterialConfiguration entity)
        {
            var model = new Models.Configuration.KeyMaterialConfiguration();
            if (entity == null)
            {
                return model;
            }

            if (!string.IsNullOrWhiteSpace(entity.SigningCertificateName))
            {
                var cert = X509.LocalMachine.My.SubjectDistinguishedName.Find(entity.SigningCertificateName).FirstOrDefault();

                if (cert == null)
                {
                    throw new InvalidOperationException("Signing certificate not found: " + entity.SigningCertificateName);
                }

                model.SigningCertificate = cert;
            }

            return model;
        }

        public static Entities.Configuration.KeyMaterialConfiguration ToEntity(this Models.Configuration.KeyMaterialConfiguration model)
        {
            var entity = new Entities.Configuration.KeyMaterialConfiguration();

            if (model.SigningCertificate != null)
            {
                entity.SigningCertificateName = model.SigningCertificate.Subject;
            }

            return entity;
        }
        #endregion

        #region WSTrustConfiguration
        public static Models.Configuration.WSTrustConfiguration ToDomainModel(this Entities.Configuration.WSTrustConfiguration entity)
        {
            return new Models.Configuration.WSTrustConfiguration
            {
                EnableClientCertificateAuthentication = entity.EnableClientCertificateAuthentication,
                Enabled = entity.Enabled,
                EnableDelegation = entity.EnableDelegation,
                EnableFederatedAuthentication = entity.EnableFederatedAuthentication,
                EnableMessageSecurity = entity.EnableMessageSecurity,
                EnableMixedModeSecurity = entity.EnableMixedModeSecurity
            };
        }

        public static Entities.Configuration.WSTrustConfiguration ToEntity(this Models.Configuration.WSTrustConfiguration model)
        {
            return new Entities.Configuration.WSTrustConfiguration
            {
                EnableClientCertificateAuthentication = model.EnableClientCertificateAuthentication,
                Enabled = model.Enabled,
                EnableDelegation = model.EnableDelegation,
                EnableFederatedAuthentication = model.EnableFederatedAuthentication,
                EnableMessageSecurity = model.EnableMessageSecurity,
                EnableMixedModeSecurity = model.EnableMixedModeSecurity
            };
        }
        #endregion

        #region FederationMetadataConfiguration
        public static Models.Configuration.FederationMetadataConfiguration ToDomainModel(this Entities.Configuration.FederationMetadataConfiguration entity)
        {
            return new Models.Configuration.FederationMetadataConfiguration
            {
                Enabled = entity.Enabled
            };
        }

        public static Entities.Configuration.FederationMetadataConfiguration ToEntity(this Models.Configuration.FederationMetadataConfiguration model)
        {
            return new Entities.Configuration.FederationMetadataConfiguration
            {
                Enabled = model.Enabled
            };
        }
        #endregion

        #region OAuth2Configuration
        public static Models.Configuration.OAuth2Configuration ToDomainModel(this Entities.Configuration.OAuth2Configuration entity)
        {
            return new Models.Configuration.OAuth2Configuration
            {
                Enabled = entity.Enabled
            };
        }
        
        public static Entities.Configuration.OAuth2Configuration ToEntity(this Models.Configuration.OAuth2Configuration model)
        {
            return new Entities.Configuration.OAuth2Configuration
            {
                Enabled = model.Enabled
            };
        }
        #endregion

        #region DiagnosticsConfiguration
        public static Models.Configuration.DiagnosticsConfiguration ToDomainModel(this Entities.Configuration.DiagnosticsConfiguration entity)
        {
            return new Models.Configuration.DiagnosticsConfiguration
            {
                EnableFederationMessageTracing = entity.EnableFederationMessageTracing
            };
        }

        public static Entities.Configuration.DiagnosticsConfiguration ToEntity(this Models.Configuration.DiagnosticsConfiguration model)
        {
            return new Entities.Configuration.DiagnosticsConfiguration
            {
                EnableFederationMessageTracing = model.EnableFederationMessageTracing
            };
        }
        #endregion

        #region Relying Party
        public static RelyingParty ToDomainModel(this RelyingParties rpEntity)
        {
            var rp = new RelyingParty 
            {
                Id = rpEntity.Id.ToString(),
                Name = rpEntity.Name,
                Realm = new Uri(rpEntity.Realm),
                ClientId = rpEntity.ClientId,
                ClientSecret = rpEntity.ClientSecret,
                ClientAuthenticationRequired = rpEntity.ClientAuthenticationRequired,
                ExtraData1 = rpEntity.ExtraData1,
                ExtraData2 = rpEntity.ExtraData2,
                ExtraData3 = rpEntity.ExtraData3
            };

            if (!string.IsNullOrWhiteSpace(rpEntity.ReplyTo))
            {
                rp.ReplyTo = new Uri(rpEntity.ReplyTo);
            }

            if (!string.IsNullOrWhiteSpace(rpEntity.EncryptingCertificate))
            {
                rp.EncryptingCertificate = new X509Certificate2(Convert.FromBase64String(rpEntity.EncryptingCertificate));
            }

            if (!string.IsNullOrWhiteSpace(rpEntity.SymmetricSigningKey))
            {
                rp.SymmetricSigningKey = Convert.FromBase64String(rpEntity.SymmetricSigningKey);
            }

            return rp;
        }

        public static RelyingParties ToEntity(this RelyingParty relyingParty)
        {
            var rpEntity = new RelyingParties
            {
                Name = relyingParty.Name,
                Realm = relyingParty.Realm.AbsoluteUri,
                ClientId = relyingParty.ClientId,
                ClientSecret = relyingParty.ClientSecret,
                ClientAuthenticationRequired = relyingParty.ClientAuthenticationRequired,
                ExtraData1 = relyingParty.ExtraData1,
                ExtraData2 = relyingParty.ExtraData2,
                ExtraData3 = relyingParty.ExtraData3,
            };

            if (!string.IsNullOrEmpty(relyingParty.Id))
            {
                rpEntity.Id = int.Parse(relyingParty.Id);
            }

            if (relyingParty.ReplyTo != null)
            {
                rpEntity.ReplyTo = relyingParty.ReplyTo.AbsoluteUri;
            }

            if (relyingParty.EncryptingCertificate != null)
            {
                rpEntity.EncryptingCertificate = Convert.ToBase64String(relyingParty.EncryptingCertificate.RawData);
            }

            if (relyingParty.SymmetricSigningKey != null && relyingParty.SymmetricSigningKey.Length != 0)
            {
                rpEntity.SymmetricSigningKey = Convert.ToBase64String(relyingParty.SymmetricSigningKey);
            }

            return rpEntity;
        }

        public static IEnumerable<RelyingParty> ToDomainModel(this List<RelyingParties> relyingParties)
        {
            return
                (from rp in relyingParties
                 select new RelyingParty
                 {
                     Id = rp.Id.ToString(),
                     Name = rp.Name,
                     Realm = new Uri(rp.Realm)
                 }).ToList();
        }
        #endregion

        #region Client Certificates
        public static List<ClientCertificate> ToDomainModel(this List<ClientCertificates> entities)
        {
            return
                (from entity in entities
                 select new ClientCertificate
                 {
                     UserName = entity.UserName,
                     Thumbprint = entity.Thumbprint,
                     Description = entity.Description
                 }
                ).ToList();
        }
        #endregion

        #region Delegation
        public static List<DelegationSetting> ToDomainModel(this List<Delegation> entities)
        {
            return
                (from entity in entities
                 select new DelegationSetting
                 {
                     UserName = entity.UserName,
                     Realm = new Uri(entity.Realm),
                     Description = entity.Description
                 }
                ).ToList();
        }
        #endregion

        #region IdentityProvider
        public static List<Models.IdentityProvider> ToDomainModel(this List<Entities.IdentityProvider> idps)
        {
            return new List<Models.IdentityProvider>(
                from idp in idps
                select new Models.IdentityProvider
                {
                    Name = idp.Name,
                    DisplayName = idp.DisplayName,
                    Type = idp.Type,
                    WSFederationEndpoint = idp.WSFederationEndpoint,
                    IssuerThumbprint = idp.IssuerThumbprint
                });
        }

        public static Models.IdentityProvider ToDomainModel(this Entities.IdentityProvider idp)
        {
            if (idp == null)
            {
                return null;
            }

            return new Models.IdentityProvider
            {
                Name = idp.Name,
                DisplayName = idp.DisplayName,
                Type = idp.Type,
                WSFederationEndpoint = idp.WSFederationEndpoint,
                IssuerThumbprint = idp.IssuerThumbprint
            };
        }
        #endregion
    }
}
