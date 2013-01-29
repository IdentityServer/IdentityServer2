using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Thinktecture.IdentityServer.Repositories;

namespace SelfHostConsoleHost
{
    class SampleUserRepository : IUserRepository
    {
        public bool ValidateUser(string userName, string password)
        {
            return (userName == password);
        }

        public bool ValidateUser(X509Certificate2 clientCertificate, out string userName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetRoles(string userName)
        {
            var roles = new List<string> {"IdentityServerUsers"};

            return roles;
        }
    }
}
