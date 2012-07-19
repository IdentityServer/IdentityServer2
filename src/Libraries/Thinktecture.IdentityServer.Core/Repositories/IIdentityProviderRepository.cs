using System.Collections.Generic;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Repositories
{
    public interface IIdentityProviderRepository
    {
        IEnumerable<IdentityProvider> GetAll();
        bool TryGet(string name, out IdentityProvider identityProvider);
    }
}
