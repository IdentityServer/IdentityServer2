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
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories.WindowsAzure
{
    public class RelyingPartyRepository : IRelyingPartyRepository
    {
        CloudStorageAccount _account;
        internal const string DefaultPartitionKey = "default";

        public RelyingPartyRepository()
            : this(TableStorageContext.StorageConnectionString)
        { }

        public RelyingPartyRepository(CloudStorageAccount account)
        {
            _account = account;
        }

        public RelyingPartyRepository(string storageConnectionString)
        {
            _account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue(storageConnectionString));
        }

        #region Runtime
        public bool TryGet(string realm, out RelyingParty relyingParty)
        {
            relyingParty = null;

            var rps = GetRelyingPartiesForServer(realm);
            var strippedRealm = realm.ToLowerInvariant().StripProtocolMoniker();

            var bestMatch =
                    (from rp in rps
                     let strippedConfig = rp.Realm.AbsoluteUri.ToLowerInvariant().StripProtocolMoniker()
                     where strippedRealm.Contains(strippedConfig)
                     orderby rp.Realm.AbsoluteUri.Length descending
                     select rp)
                    .FirstOrDefault();

            if (bestMatch != null)
            {
                relyingParty = bestMatch;
                return true;
            }

            return false;
        }

        private List<RelyingParty> GetRelyingPartiesForServer(string realm)
        {
            var rps = new List<RelyingParty>();
            var realmUri = new Uri(realm);

            var result = from rp in NewContext.RelyingParties
                         where rp.PartitionKey == DefaultPartitionKey &&
                               rp.RealmHost.Equals(realmUri.DnsSafeHost, StringComparison.OrdinalIgnoreCase)
                         select rp;

            foreach (var rp in result)
            {
                rps.Add(rp.ToDomainModel());
            }

            return rps;
        }
        #endregion

        #region Management
        public bool SupportsWriteAccess
        {
            get { return true; }
        }

        public IEnumerable<RelyingParty> List(int pageIndex, int pageSize)
        {
            if (pageIndex != -1 && pageSize != -1)
            {
                return All().Skip(pageIndex * pageSize).Take(pageSize).ToList().ToDomainModel();
            }

            return All().ToDomainModel();
        }

        public RelyingParty Get(string id)
        {
            var entity = (from rp in NewContext.RelyingParties
                          where rp.PartitionKey == DefaultPartitionKey &&
                                rp.RowKey == id
                          select rp)
                         .Single();

            return entity.ToDomainModel();
        }

        public void Add(RelyingParty relyingParty)
        {
            NewContext.AddRelyingParty(relyingParty.ToEntity());
        }

        public void Update(RelyingParty relyingParty)
        {
            NewContext.UpdateRelyingParty(relyingParty.ToEntity(relyingParty.Id));
        }

        public void Delete(string id)
        {
            NewContext.DeleteRelyingParty(DefaultPartitionKey, id);
        }

        private List<RelyingPartyEntity> All()
        {
            var rpEntities = from rp in NewContext.RelyingParties
                             where rp.PartitionKey == DefaultPartitionKey
                             select rp;

            return rpEntities.ToList().OrderBy(rp => rp.PartitionKey).ToList();
        }
        #endregion

        private TableStorageContext NewContext
        {  
            get
            {
                var context = new TableStorageContext(_account.TableEndpoint.AbsoluteUri, _account.Credentials);
                return context;
            }
        }




        
    }
}