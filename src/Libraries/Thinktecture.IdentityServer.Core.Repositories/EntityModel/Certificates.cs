/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class Certificates
    {
        [Key]
        public string Name { get; set; }

        public string SubjectDistinguishedName { get; set; }
    }
}
