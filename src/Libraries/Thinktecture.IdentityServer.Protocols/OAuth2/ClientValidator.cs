using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Protocols.OAuth2
{
    public class ClientValidator
    {
        [Import]
        public IClientsRepository Clients { get; set; }

        public ClientValidator()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public ClientValidator(IClientsRepository clients)
        {
            Clients = clients;
        }

        public bool ValidateClient(string clientId, string clientSecret)
        {
            return Clients.ValidateClient(clientId, clientSecret);
        }
    }
}
