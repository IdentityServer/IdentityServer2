/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Security.Cryptography.X509Certificates;

namespace Thinktecture.IdentityServer.Models
{
    public class CertificateConfiguration
    {
        public string SubjectDistinguishedName { get; set; }
        public X509Certificate2 Certificate { get; set; }
    }
}
