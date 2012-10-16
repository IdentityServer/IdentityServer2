using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories
{
    public interface IClientsRepository
    {
        // needed for core issuance logic
        bool ValidateClient(string clientId, string clientSecret);
        bool TryGetClient(string clientId, out Client client);
    }
}