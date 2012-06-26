using Microsoft.WindowsAzure.StorageClient;

namespace Thinktecture.IdentityServer.Repositories.WindowsAzure
{
    public class UserAccountEntity : TableServiceEntity
    {
        public const string EntityKind = "Account";

        public string Kind { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string InternalRoles { get; set; }
    }
}
