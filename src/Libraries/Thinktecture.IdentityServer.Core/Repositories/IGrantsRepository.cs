using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories
{
    public interface IGrantsRepository
    {
        void Add(Grant grant);
        Grant Get(string id);
        void Delete(string id);
    }
}
