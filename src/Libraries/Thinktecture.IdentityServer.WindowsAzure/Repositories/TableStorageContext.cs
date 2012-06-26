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
using System.Data.Services.Client;
using System.Linq;
using Microsoft.IdentityModel.Claims;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Thinktecture.IdentityServer.Repositories.WindowsAzure
{
    public class TableStorageContext : TableServiceContext
    {
        internal const string RelyingPartiesTable     = "RelyingParties";
        internal const string ClientCertificatesTable = "ClientCertificates";
        internal const string DelegationTable         = "Delegation";
        internal const string UsersTable              = "Users";
        internal const string StorageConnectionString = "StorageConnectionString";

        #region Structural
        static TableStorageContext()
        {
            try
            {
                SetupTables();
            }
            catch { }
        }

        public static void SetupTables()
        {
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue(StorageConnectionString));
            SetupTables(account);
        }

        public static void SetupTables(string connectionString)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            SetupTables(account);
        }

        public static void SetupTables(CloudStorageAccount account)
        {
            var client = account.CreateCloudTableClient();

            client.CreateTableIfNotExist(RelyingPartiesTable);
            client.CreateTableIfNotExist(ClientCertificatesTable);
            client.CreateTableIfNotExist(DelegationTable);
            client.CreateTableIfNotExist(UsersTable);
        }

        public TableStorageContext(string baseAddress, StorageCredentials credentials)
            : base(baseAddress, credentials)
        { }

        public TableStorageContext(string connectionString)
            : base(GetBaseAddress(connectionString), GetCredentials(connectionString))
        { }

        private static string GetBaseAddress(string connectionString)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            return account.TableEndpoint.AbsoluteUri;
        }

        private static StorageCredentials GetCredentials(string connectionString)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            return account.Credentials;
        }
        #endregion

        #region Users
        public IQueryable<UserAccountEntity> UserAccounts
        {
            get
            {
                return CreateQuery<UserAccountEntity>(UsersTable).Where(t => t.RowKey == UserAccountEntity.EntityKind).AsTableServiceQuery();
            }
        }

        public IQueryable<UserClaimEntity> UserClaims
        {
            get
            {
                return CreateQuery<UserClaimEntity>(UsersTable).Where(t => t.Kind == UserClaimEntity.EntityKind).AsTableServiceQuery();
            }
        }

        public IEnumerable<string> GetUsers()
        {
            var users = (from u in UserAccounts
                         select u)
                        .ToList();

            return (from item in users
                    select item.PartitionKey)
                   .ToList();
        }

        public IEnumerable<Claim> GetUserClaims(string userName)
        {
            var userClaims = (from u in UserClaims
                              where u.PartitionKey == userName.ToLowerInvariant()
                              select new Claim(u.ClaimType, u.Value))
                             .ToList();

            return userClaims;
        }

        public void AddUserAccount(string userName, string password, bool isAdministrator)
        {
            string hash;
            string salt;
            new PasswordCrypto().HashPassword(password, out hash, out salt);

            var internalRoles = Constants.Roles.IdentityServerUsers;
            if (isAdministrator)
            {
                internalRoles += "," + Constants.Roles.IdentityServerAdministrators;
            }

            AddUserAccount(userName, hash, salt, internalRoles);
        }

        public void AddUserAccount(string userName, string passwordHash, string salt, string internalRoles)
        {
            var entity = new UserAccountEntity
            {
                PartitionKey = userName.ToLowerInvariant(),
                RowKey = UserAccountEntity.EntityKind,
                Kind = UserAccountEntity.EntityKind,
                PasswordHash = passwordHash,
                Salt = salt,
                InternalRoles = internalRoles
            };

            AddEntity(entity, UsersTable);
        }

        public void DeleteUserAccount(string userName)
        {
            var user = (from u in UserAccounts
                        where u.PartitionKey == userName.ToLowerInvariant()
                        select u)
                       .Single();
            
            DeleteObject(user);
            SaveChangesWithRetries();

            DeleteUserClaims(userName);
        }

        public void DeleteUserClaims(string userName)
        {
            var userClaims = from u in UserClaims
                             where u.PartitionKey == userName.ToLowerInvariant()
                             select u;

            userClaims.ToList().ForEach(u => DeleteObject(u));
            SaveChangesWithRetries(SaveChangesOptions.Batch);
        }

        public void AddUserClaims(string userName, IEnumerable<Tuple<string, string>> claimsStrings)
        {
            var claims = (from c in claimsStrings.ToList()
                          select new Claim(c.Item1, c.Item2))
                         .ToList();

            AddUserClaims(userName, claims);
        }

        public void AddUserClaims(string userName, IEnumerable<Claim> claims)
        {
            var userClaims = new List<UserClaimEntity>(
                from c in claims.ToList()
                select new UserClaimEntity
                {
                    PartitionKey = userName.ToLowerInvariant(),
                    RowKey = Guid.NewGuid().ToString(),
                    Kind = UserClaimEntity.EntityKind,
                    ClaimType = c.ClaimType,
                    Value = c.Value
                });

            userClaims.ForEach(c => AddObject(UsersTable, c));
            SaveChangesWithRetries(SaveChangesOptions.Batch);
        }
        #endregion

        #region Relying Parties
        public IQueryable<RelyingPartyEntity> RelyingParties
        {
            get
            {
                return CreateQuery<RelyingPartyEntity>(RelyingPartiesTable).AsTableServiceQuery();
            }
        }

        public void AddRelyingParty(RelyingPartyEntity entity)
        {
            AddEntity(entity, RelyingPartiesTable);
        }

        public void DeleteRelyingParty(string partitionKey, string id)
        {
            var entity = new RelyingPartyEntity
            {
                PartitionKey = partitionKey,
                RowKey = id
            };

            AttachTo(RelyingPartiesTable, entity, "*");
            DeleteObject(entity);
            SaveChangesWithRetries();
        }

        public void UpdateRelyingParty(RelyingPartyEntity entity)
        {
            AttachTo(RelyingPartiesTable, entity, "*");
            UpdateObject(entity);
            SaveChangesWithRetries();
        }
        #endregion

        #region Client Certificates
        public IQueryable<ClientCertificateEntity> ClientCertificates
        {
            get
            {
                return CreateQuery<ClientCertificateEntity>(ClientCertificatesTable).AsTableServiceQuery();
            }
        }

        public List<ClientCertificateEntity> AllClientCertificates(string partitionKey)
        {
            var entities = (from e in ClientCertificates
                            where e.PartitionKey == partitionKey
                            select e)
                           .ToList();

            return entities.ToList();
        }
        

        public void AddClientCertificate(ClientCertificateEntity entity)
        {
            AddEntity(entity, ClientCertificatesTable);
        }

        public void DeleteClientCertificate(ClientCertificateEntity entity)
        {
            AttachTo(ClientCertificatesTable, entity, "*");
            DeleteObject(entity);
            SaveChangesWithRetries();
        }
        #endregion

        #region Delegation
        public IQueryable<DelegationEntity> Delegation
        {
            get
            {
                return CreateQuery<DelegationEntity>(DelegationTable).AsTableServiceQuery();
            }
        }

        public List<DelegationEntity> AllDelegations(string partitionKey)
        {
            var entities = (from e in Delegation
                            where e.PartitionKey == partitionKey
                            select e)
                           .ToList();

            return entities.ToList();
        }

        public List<DelegationEntity> AllDelegations(string partitionKey, string userName)
        {
            var entities = (from e in Delegation
                            where e.PartitionKey == partitionKey &&
                                  e.UserName.Equals(userName, System.StringComparison.OrdinalIgnoreCase)
                            select e)
                           .ToList();

            return entities.ToList();
        }

        public void AddDelegation(DelegationEntity entity)
        {
            AddEntity(entity, DelegationTable);
        }

        public void DeleteDelegation(string partitionKey, string id)
        {
            var entity = new DelegationEntity
            {
                PartitionKey = partitionKey,
                RowKey = id
            };

            AttachTo(DelegationTable, entity, "*");
            DeleteObject(entity);
            SaveChangesWithRetries();
        }
        #endregion

        private void AddEntity(object entity, string table)
        {
            AddObject(table, entity);
            SaveChangesWithRetries();
        }
    }
}
