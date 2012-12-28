using System;
using System.Collections.Generic;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.Samples
{
    class RelyingPartyRepository : IRelyingPartyRepository
    {
        public void Add(IdentityServer.Models.RelyingParty relyingParty)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IdentityServer.Models.RelyingParty Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IdentityServer.Models.RelyingParty> List(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public bool SupportsWriteAccess
        {
            get { throw new NotImplementedException(); }
        }

        public bool TryGet(string realm, out IdentityServer.Models.RelyingParty relyingParty)
        {
            relyingParty = new IdentityServer.Models.RelyingParty
            {
                Enabled = true,
                Realm = new Uri(realm),
                Name = "InMemory realm.",
                SymmetricSigningKey = Convert.FromBase64String("L6aMge6UHG5IJ+Ah10Nhw9wsmkWC9ZBUEHT2lciAwSw=")
            };

            return true;
        }

        public void Update(IdentityServer.Models.RelyingParty relyingParty)
        {
            throw new NotImplementedException();
        }
    }
}
