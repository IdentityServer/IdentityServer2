using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class OAuthClientViewModel
    {
        private Repositories.IClientsRepository clientsRepository;
        public OAuthClientInputModel[] Clients
        {
            get;
            private set;
        }

        public OAuthClientViewModel(Repositories.IClientsRepository clientsRepository)
        {
            // TODO: Complete member initialization
            this.clientsRepository = clientsRepository;
            this.Clients = this.clientsRepository.GetAll().Select(x=>new OAuthClientInputModel{Name=x.Name, ID=x.ID}).ToArray();
        }
    }

    public class OAuthClientInputModel
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public bool Delete { get; set; }
    }

}