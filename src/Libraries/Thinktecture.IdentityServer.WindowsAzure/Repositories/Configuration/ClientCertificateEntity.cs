/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using Microsoft.WindowsAzure.StorageClient;

namespace Thinktecture.IdentityServer.Repositories.WindowsAzure
{
    public class ClientCertificateEntity : TableServiceEntity
    {
        // partition key: default
        // rowkey: thumbprint

        public string UserName { get; set; }
        public string Description { get; set; }

        public ClientCertificateEntity()
        { }

        public ClientCertificateEntity(string thumbprint, string userName, string description)
        {
            PartitionKey = RelyingPartyRepository.DefaultPartitionKey;
            RowKey = thumbprint.ToLowerInvariant();

            UserName = userName.ToLowerInvariant();
            Description = description;
        }
    }
}
