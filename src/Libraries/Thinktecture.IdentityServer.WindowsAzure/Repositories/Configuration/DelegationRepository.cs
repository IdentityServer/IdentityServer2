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
    public class DelegationRepository : IDelegationRepository
    {
        CloudStorageAccount _account;
        internal const string DefaultPartitionKey = "default";

        public DelegationRepository()
            : this(TableStorageContext.StorageConnectionString)
        { }

        public DelegationRepository(CloudStorageAccount account)
        {
            _account = account;
        }

        public DelegationRepository(string storageConnectionString)
        {
            _account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue(storageConnectionString));
        }

        #region Runtime
        public bool IsDelegationAllowed(string userName, string realm)
        {
            var context = NewContext;

            context.IgnoreResourceNotFoundException = true;

            var result = (from d in context.Delegation
                          where d.PartitionKey == DefaultPartitionKey &&
                                d.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) &&
                                d.Realm.Equals(realm, StringComparison.OrdinalIgnoreCase)
                          select d)
                         .FirstOrDefault();

            if (result != null)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Management
        public void Add(DelegationSetting setting)
        {
            NewContext.AddDelegation(setting.ToEntity());
        }

        public void Delete(DelegationSetting setting)
        {
            var entity = (from e in NewContext.Delegation
                          where e.PartitionKey == DefaultPartitionKey &&
                                e.UserName.Equals(setting.UserName, StringComparison.OrdinalIgnoreCase) &&
                                e.Realm.Equals(setting.Realm.AbsoluteUri, StringComparison.OrdinalIgnoreCase)
                          select e)
                         .Single();

            NewContext.DeleteDelegation(entity.PartitionKey, entity.RowKey);
        }

        public IEnumerable<string> GetAllUsers(int pageIndex, int pageSize)
        {
            var all = (from e in NewContext.AllDelegations(DefaultPartitionKey)
                       select e.UserName);

            return all.Distinct().ToList();
        }

        public IEnumerable<DelegationSetting> GetDelegationSettingsForUser(string userName)
        {
            return NewContext.AllDelegations(DefaultPartitionKey, userName).ToDomainModel();
        }

        public bool SupportsWriteAccess
        {
            get { return true; }
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
