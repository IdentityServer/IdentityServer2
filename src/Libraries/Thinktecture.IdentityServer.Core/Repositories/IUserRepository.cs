/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Thinktecture.IdentityServer.Repositories
{
    public interface IUserRepository
    {
        bool ValidateUser(string userName, string password);
        bool ValidateUser(X509Certificate2 clientCertificate, out string userName);
        IEnumerable<string> GetRoles(string userName);        
    }
}