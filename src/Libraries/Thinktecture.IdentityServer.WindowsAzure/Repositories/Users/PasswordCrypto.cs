using System;
using System.Security.Cryptography;
using Thinktecture.IdentityModel;

namespace Thinktecture.IdentityServer.Repositories.WindowsAzure
{
    internal class PasswordCrypto
    {
        internal const int IterationCount = 5000;
        internal const int HashSize = 16;
        internal const int SaltSize = 16;

        public void HashPassword(string password, out string hash, out string salt)
        {
            var rfc = new Rfc2898DeriveBytes(password, SaltSize, IterationCount);
            hash = Convert.ToBase64String(rfc.GetBytes(HashSize));
            salt = Convert.ToBase64String(rfc.Salt);
        }

        public string HashPasswordWithSalt(string password, string salt)
        {
            var rfc = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), IterationCount);
            return Convert.ToBase64String(rfc.GetBytes(HashSize));
        }

        public bool ValidatePassword(string password, string hash, string salt)
        {
            var calculatedHash = HashPasswordWithSalt(password, salt);
            return ObfuscatingComparer.IsEqual(hash, calculatedHash);
        }
    }
}
