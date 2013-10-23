using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.TokenService;

namespace MembershipRebootUserRepository
{
    public class IdentityRepository : 
        IUserRepository,
        IUserManagementRepository,
        IClientCertificatesRepository,
        IClaimsRepository
    {
        UserAccountService userSvc;
        GroupService groupSvc;

        public IdentityRepository()
        {
            var settings = SecuritySettings.FromConfiguration();
            settings.RequireAccountVerification = false;
            var config = new MembershipRebootConfiguration(settings);
            this.userSvc = new UserAccountService(config, new BrockAllen.MembershipReboot.Ef.DefaultUserAccountRepository());
            this.groupSvc = new GroupService(new BrockAllen.MembershipReboot.Ef.DefaultGroupRepository());
        }

        public IdentityRepository(UserAccountService userSvc, GroupService groupSvc)
        {
            this.userSvc = userSvc;
            this.groupSvc = groupSvc;
        }

        #region IUserRepository
        public IEnumerable<string> GetRoles(string userName)
        {
            var user = userSvc.GetByUsername(userName);
            if (user != null)
            {
                return user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x=>x.Value);
            }
            return Enumerable.Empty<string>();
        }

        public bool ValidateUser(System.Security.Cryptography.X509Certificates.X509Certificate2 clientCertificate, out string userName)
        {
            UserAccount user;
            if (userSvc.AuthenticateWithCertificate(clientCertificate, out user))
            {
                userName = user.Username;
                return true;
            }

            userName = null;
            return false;
        }

        public bool ValidateUser(string userName, string password)
        {
            return userSvc.Authenticate(userName, password);
        }
        #endregion

        #region IUserManagementRepository

        public void CreateRole(string roleName)
        {
            groupSvc.Create(roleName);
        }

        public void CreateUser(string userName, string password, string email = null)
        {
            userSvc.CreateAccount(userName, password, email);
        }

        public void DeleteRole(string roleName)
        {
            var grp = groupSvc.GetAll().Where(x=>x.Name == roleName).SingleOrDefault();
            if (grp != null)
            {
                groupSvc.Delete(grp.ID);
            }
        }

        public void DeleteUser(string userName)
        {
            var user = userSvc.GetByUsername(userName);
            if (user != null)
            {
                userSvc.DeleteAccount(user.ID);
            }
        }

        public IEnumerable<string> GetRoles()
        {
            return groupSvc.GetAll().Select(x => x.Name);
        }

        public IEnumerable<string> GetRolesForUser(string userName)
        {
            var user = userSvc.GetByUsername(userName);
            if (user != null)
            {
                return user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);
            }
            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetUsers(int start, int count, out int totalCount)
        {
            var query =
                from u in userSvc.GetAll().OrderBy(x=>x.ID)
                select u.Username;
            totalCount = query.Count();
            return query.Skip(start).Take(count).ToArray();
        }

        public IEnumerable<string> GetUsers(string filter, int start, int count, out int totalCount)
        {
            var query =
                from u in userSvc.GetAll().OrderBy(x=>x.ID)
                where u.Username.Contains(filter)
                select u.Username;
            totalCount = query.Count();
            return query.Skip(start).Take(count).ToArray();
        }

        public void SetPassword(string userName, string password)
        {
            var user = userSvc.GetByUsername(userName);
            if (user != null)
            {
                userSvc.SetPassword(user.ID, password);
            }
        }

        public void SetRolesForUser(string userName, IEnumerable<string> roles)
        {
            var user = userSvc.GetByUsername(userName);
            if (user != null)
            {
                user.RemoveClaim(ClaimTypes.Role);
                if (roles != null)
                {
                    foreach (var role in roles)
                    {
                        user.AddClaim(ClaimTypes.Role, role);
                    }
                }
                userSvc.Update(user);
            }
        }
        #endregion

        #region IClientCertificatesRepository
        public void Add(Thinktecture.IdentityServer.Models.ClientCertificate certificate)
        {
            var user = userSvc.GetByUsername(certificate.UserName);
            if (user != null)
            {
                user.AddCertificate(certificate.Thumbprint, certificate.Description);
                userSvc.Update(user);
            }
        }

        public void Delete(Thinktecture.IdentityServer.Models.ClientCertificate certificate)
        {
            var user = userSvc.GetByUsername(certificate.UserName);
            if (user != null)
            {
                user.RemoveCertificate(certificate.Thumbprint);
                userSvc.Update(user);
            }
        }

        public IEnumerable<Thinktecture.IdentityServer.Models.ClientCertificate> GetClientCertificatesForUser(string userName)
        {
            var user = userSvc.GetByUsername(userName);
            if (user != null)
            {
                return user.Certificates.Select(x => new Thinktecture.IdentityServer.Models.ClientCertificate() { UserName = user.Username, Thumbprint = x.Thumbprint, Description = x.Subject });
            }
            return Enumerable.Empty<Thinktecture.IdentityServer.Models.ClientCertificate>();
        }

        public IEnumerable<string> List(int pageIndex, int pageSize)
        {
            var query =
                from u in userSvc.GetAll().OrderBy(x => x.ID)
                where u.Certificates.Any()
                select u.Username;
            if (pageIndex >= 0 && pageSize >= 0)
            {
                return query.Skip(pageIndex).Take(pageSize);
            }
            else
            {
                return query;
            }
        }

        public bool SupportsWriteAccess
        {
            get { return true; }
        }

        public bool TryGetUserNameFromThumbprint(System.Security.Cryptography.X509Certificates.X509Certificate2 certificate, out string userName)
        {
            UserAccount user;
            if (userSvc.AuthenticateWithCertificate(certificate, out user))
            {
                userName = user.Username;
                return true;
            }
            userName = null;
            return false;
        }
        #endregion

        #region IClaimsRepository
        public IEnumerable<System.Security.Claims.Claim> GetClaims(
            ClaimsPrincipal principal, RequestDetails requestDetails)
        {
            var user = userSvc.GetByUsername(principal.Identity.Name);
            if (user == null) throw new ArgumentException("Invalid Username");

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString("D")));
            if (!String.IsNullOrWhiteSpace(user.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
            }
            if (!String.IsNullOrWhiteSpace(user.MobilePhoneNumber))
            {
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.MobilePhoneNumber));
            }
            //var x509 = from c in user.Certificates
            //           select new Claim(ClaimTypes.X500DistinguishedName, c.Subject);
            //claims.AddRange(x509);
            var otherClaims =
                (from uc in user.Claims
                 select new Claim(uc.Type, uc.Value)).ToList();
            claims.AddRange(otherClaims);

            return claims;
        }

        public IEnumerable<string> GetSupportedClaimTypes()
        {
            var query =
                from u in userSvc.GetAll()
                from c in u.Claims
                select c.Type;
            
            return
                new string[] { ClaimTypes.Name, ClaimTypes.Email, ClaimTypes.MobilePhone, ClaimTypes.Role }
                .Union(query.Distinct()).Distinct();
        }
        #endregion
    }
}
