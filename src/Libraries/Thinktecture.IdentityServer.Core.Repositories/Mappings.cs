/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Thinktecture.IdentityServer.Models;
using Models = Thinktecture.IdentityServer.Models;
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
        #endregion


        #region Relying Party
        public static RelyingParty ToDomainModel(this RelyingParties rpEntity)
        {
            var rp = new RelyingParty 
            {
                Id = rpEntity.Id.ToString(),
                Name = rpEntity.Name,
                Realm = new Uri(rpEntity.Realm),
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
