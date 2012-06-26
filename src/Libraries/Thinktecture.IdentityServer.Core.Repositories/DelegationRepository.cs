/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories.Sql
{    
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DelegationRepository : IDelegationRepository
    {
        #region Runtime
        public bool IsDelegationAllowed(string userName, string realm)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var record = (from entry in entities.Delegation
                              where entry.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) &&
                                    entry.Realm.Equals(realm, StringComparison.OrdinalIgnoreCase)
                              select entry).FirstOrDefault();

                return (record != null);
            }
        }
        #endregion

        #region Management
        public bool SupportsWriteAccess
        {
            get { return true; }
        }

        public IEnumerable<string> GetAllUsers(int pageIndex, int pageSize)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var users =
                    (from user in entities.Delegation
                     orderby user.UserName
                     select user.UserName)
                    .Distinct();

                if (pageIndex != -1 && pageSize != -1)
                {
                    users = users.Skip(pageIndex * pageSize).Take(pageSize);
                }

                return users.ToList();
            }
        }

        public IEnumerable<DelegationSetting> GetDelegationSettingsForUser(string userName)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var settings =
                     from record in entities.Delegation
                     where record.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)
                     select record;

                return settings.ToList().ToDomainModel();
            }
        }

        public void Add(DelegationSetting setting)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var entity = new Delegation
                {
                    UserName = setting.UserName,
                    Realm = setting.Realm.AbsoluteUri,
                    Description = setting.Description
                };

                entities.Delegation.Add(entity);
                entities.SaveChanges();
            }
        }

        public void Delete(DelegationSetting setting)
        {
            using (var entities = IdentityServerConfigurationContext.Get())
            {
                var record =
                    (from entry in entities.Delegation
                     where entry.UserName.Equals(setting.UserName, StringComparison.OrdinalIgnoreCase) &&
                           entry.Realm.Equals(setting.Realm.AbsoluteUri, StringComparison.OrdinalIgnoreCase)
                     select entry)
                    .Single();

                entities.Delegation.Remove(record);
                entities.SaveChanges();
            }
        }
        #endregion
    }
}
