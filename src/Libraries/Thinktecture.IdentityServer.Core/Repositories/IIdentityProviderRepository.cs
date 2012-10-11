using System.Collections.Generic;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories
{
    public interface IIdentityProviderRepository
    {
        IEnumerable<IdentityProvider> GetAll();
        void Add(IdentityProvider item);
        void Update(IdentityProvider item);
        void Delete(string name);
        bool TryGet(string name, out IdentityProvider identityProvider);
    }
}
