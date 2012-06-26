/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories.WindowsAzure
{
    public static class Extensions
    {
        #region Relying Party
        public static RelyingPartyEntity ToEntity(this RelyingParty model)
        {
            return model.ToEntity(Guid.NewGuid().ToString());
        }

        public static RelyingPartyEntity ToEntity(this RelyingParty model, string id)
        {
            return model.ToEntity(id, RelyingPartyRepository.DefaultPartitionKey);
        }

        public static RelyingPartyEntity ToEntity(this RelyingParty model, string id, string partitionKey)
        {
            var entity = new RelyingPartyEntity
            {
                PartitionKey = partitionKey,
                RowKey = id,
                RealmHost = model.Realm.DnsSafeHost.ToLowerInvariant(),
                RealmPath = model.Realm.PathAndQuery.ToLowerInvariant(),
                Description = model.Name,
                ExtraData1 = model.ExtraData1 ?? "",
                ExtraData2 = model.ExtraData2 ?? "",
                ExtraData3 = model.ExtraData3 ?? "",
            };

            if (model.ReplyTo != null)
            {
                entity.ReplyToAddress = model.ReplyTo.AbsoluteUri;
            }
            else
            {
                entity.ReplyToAddress = "";
            }

            if (model.EncryptingCertificate != null)
            {
                entity.EncryptingCertificate = Convert.ToBase64String(model.EncryptingCertificate.RawData);
            }

            if (model.SymmetricSigningKey != null && model.SymmetricSigningKey.Length != 0)
            {
                entity.SymmetricSigningKey = Convert.ToBase64String(model.SymmetricSigningKey);
            }
            else
            {
                entity.SymmetricSigningKey = "";
            }

            return entity;
        }

        public static RelyingParty ToDomainModel(this RelyingPartyEntity entity)
        {
            var model = new RelyingParty
            {
                Id = entity.RowKey,
                Realm = new Uri("http://" + entity.RealmHost + entity.RealmPath),
                Name = entity.Description,
                ExtraData1 = entity.ExtraData1,
                ExtraData2 = entity.ExtraData2,
                ExtraData3 = entity.ExtraData3
            };

            if (entity.EncryptingCertificate != null)
            {
                model.EncryptingCertificate = new X509Certificate2(Convert.FromBase64String(entity.EncryptingCertificate));
            }

            if (!string.IsNullOrWhiteSpace(entity.ReplyToAddress))
            {
                model.ReplyTo = new Uri(entity.ReplyToAddress);
            }

            if (!string.IsNullOrWhiteSpace(entity.SymmetricSigningKey))
            {
                model.SymmetricSigningKey = Convert.FromBase64String(entity.SymmetricSigningKey);
            }

            return model;
        }

        public static List<RelyingParty> ToDomainModel(this List<RelyingPartyEntity> entities)
        {
            return (from e in entities
                    select e.ToDomainModel())
                   .ToList();
        }

        public static string StripProtocolMoniker(this string uriString)
        {
            var uri = new Uri(uriString);
            string stripped = uri.AbsoluteUri.Substring(uri.Scheme.Length + 3);
            return stripped;
        }
        #endregion

        #region Client Certificates
        public static List<ClientCertificate> ToDomainModel(this List<ClientCertificateEntity> entities)
        {
            return (from e in entities
                    select e.ToDomainModel())
                   .ToList();
        }

        public static ClientCertificate ToDomainModel(this ClientCertificateEntity entity)
        {
            var cc = new ClientCertificate
            {
                UserName = entity.UserName,
                Thumbprint = entity.RowKey,
                Description = entity.Description
            };

            return cc;
        }

        public static ClientCertificateEntity ToEntity(this ClientCertificate model)
        {
            return model.ToEntity(ClientCertificatesRepository.DefaultPartitionKey);
        }

        public static ClientCertificateEntity ToEntity(this ClientCertificate model, string partitionKey)
        {
            return new ClientCertificateEntity(model.Thumbprint, model.UserName, model.Description) 
                { 
                    PartitionKey = partitionKey 
                };
        }
        #endregion

        #region Delegation
        public static DelegationEntity ToEntity(this DelegationSetting model)
        {
            return model.ToEntity(DelegationRepository.DefaultPartitionKey);
        }

        public static DelegationEntity ToEntity(this DelegationSetting model, string partitionKey)
        {
            var entity = new DelegationEntity
            {
                PartitionKey = partitionKey,
                RowKey = Guid.NewGuid().ToString(),

                UserName = model.UserName.ToLowerInvariant(),
                Realm = model.Realm.AbsoluteUri.ToLowerInvariant(),
                Description = model.Description
            };

            return entity;
        }

        public static List<DelegationSetting> ToDomainModel(this List<DelegationEntity> entities)
        {
            return (from e in entities
                    select e.ToDomainModel())
                   .ToList();
        }

        public static DelegationSetting ToDomainModel(this DelegationEntity entity)
        {
            return new DelegationSetting
            {
                UserName = entity.UserName,
                Realm = new Uri(entity.Realm),
                Description = entity.Description
            };
        }
        #endregion
    }
}
