using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Claims;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Thinktecture.IdentityServer.TokenService;
using System.ComponentModel.Composition;

namespace Thinktecture.IdentityServer.Repositories.WindowsAzure
{
    public class TableStorageClaimsRepository : IClaimsRepository
    {
        CloudStorageAccount _account;

        public TableStorageClaimsRepository()
            : this(TableStorageContext.StorageConnectionString)
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public TableStorageClaimsRepository(string storageConnectionString)
        {
            _account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue(storageConnectionString));
        }

        public IEnumerable<Claim> GetClaims(IClaimsPrincipal principal, RequestDetails requestDetails)
        {
            var claims = from c in NewContext.UserClaims
                         where c.PartitionKey == principal.Identity.Name.ToLower() &&
                               c.Kind == UserClaimEntity.EntityKind
                         select new Claim(c.ClaimType, c.Value);

            return claims.ToList();
        }

        public IEnumerable<string> GetSupportedClaimTypes()
        {
            return new string[] { ClaimTypes.Name, ClaimTypes.Role, ClaimTypes.Email };
        }

        private bool TryGetUserAccount(string userName, out UserAccountEntity account)
        {
            var context = NewContext;

            // check if user exists
            account = (from c in context.UserAccounts
                       where c.PartitionKey == userName.ToLowerInvariant() &&
                             c.RowKey == UserAccountEntity.EntityKind
                       select c).FirstOrDefault();

            return (account != null);
        }

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