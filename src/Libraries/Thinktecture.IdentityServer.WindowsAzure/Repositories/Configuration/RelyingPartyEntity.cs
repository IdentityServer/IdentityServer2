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
    public class RelyingPartyEntity : TableServiceEntity
    {
        // partition key = default
        // rowkey = unique id

        public string RealmHost { get; set; }
        public string RealmPath { get; set; }
        public string Description { get; set; }
        public string ReplyToAddress { get; set; }
        public string EncryptingCertificate { get; set; }
        public string SymmetricSigningKey { get; set; }
        public string ExtraData1 { get; set; }
        public string ExtraData2 { get; set; }
        public string ExtraData3 { get; set; }
    }
}
