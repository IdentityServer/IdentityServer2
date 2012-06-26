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
    public class DelegationEntity : TableServiceEntity
    {
        // partition key: default
        // rowkey: unique id

        public string UserName { get; set; }
        public string Realm { get; set; }
        public string Description { get; set; }
    }
}
