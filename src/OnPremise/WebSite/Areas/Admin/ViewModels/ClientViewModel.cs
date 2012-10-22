using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class ClientViewModel
    {
        private Repositories.IClientsRepository clientsRepository;
        public ClientInputModel[] Clients
        {
            get;
            private set;
        }

        public ClientViewModel(Repositories.IClientsRepository clientsRepository)
        {
            // TODO: Complete member initialization
            this.clientsRepository = clientsRepository;
            this.Clients = this.clientsRepository.GetAll().Select(x=>new ClientInputModel{Name=x.Name, ID=x.ID}).ToArray();
        }
    }

    public class ClientInputModel
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public bool Delete { get; set; }
    }

}