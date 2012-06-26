/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories.WindowsAzure
{
    public class ClientCertificatesRepository : IClientCertificatesRepository
    {
        CloudStorageAccount _account;
        internal const string DefaultPartitionKey = "default";

        public ClientCertificatesRepository()
            : this(TableStorageContext.StorageConnectionString)
        { }

        public ClientCertificatesRepository(CloudStorageAccount account)
        {
            _account = account;
        }

        public ClientCertificatesRepository(string storageConnectionString)
        {
            _account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue(storageConnectionString));
        }

        #region Runtime
        public bool TryGetUserNameFromThumbprint(X509Certificate2 certificate, out string userName)
        {
            userName = null;
            var thumbprint = certificate.Thumbprint.ToLowerInvariant();
            var context = NewContext;
            context.IgnoreResourceNotFoundException = true;

            var entity = (from cc in context.ClientCertificates
                          where cc.PartitionKey == DefaultPartitionKey &&
                                cc.RowKey == thumbprint
                          select cc)
                         .SingleOrDefault();

            if (entity != null && !string.IsNullOrEmpty(entity.UserName))
            {
                userName = entity.UserName;
                return true;
            }

            return false;
        }
        #endregion

        #region Management
        public bool SupportsWriteAccess
        {
            get { return true; }
        }

        public IEnumerable<string> List(int pageIndex, int pageSize)
        {
            var all = NewContext.AllClientCertificates(DefaultPartitionKey);

            return (from cc in all
                    select cc.UserName)
                   .Distinct().ToList();
        }

        public IEnumerable<ClientCertificate> GetClientCertificatesForUser(string userName)
        {
            var user = userName.ToLowerInvariant();

            var certs = from e in NewContext.ClientCertificates
                        where e.PartitionKey == DefaultPartitionKey &&
                              e.UserName == user
                        select e;

            return certs.ToList().ToDomainModel();
        }

        public void Add(ClientCertificate certificate)
        {
            NewContext.AddClientCertificate(certificate.ToEntity());
        }

        public void Delete(ClientCertificate certificate)
        {
            NewContext.DeleteClientCertificate(certificate.ToEntity());
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
