using Microsoft.WindowsAzure.StorageClient;

namespace Thinktecture.IdentityServer.Repositories.WindowsAzure
{
    public class UserClaimEntity : TableServiceEntity
    {
        public const string EntityKind = "Claim";

        public string Kind { get; set; }
        public string ClaimType { get; set; }
        public string Value { get; set; }
    }
}
