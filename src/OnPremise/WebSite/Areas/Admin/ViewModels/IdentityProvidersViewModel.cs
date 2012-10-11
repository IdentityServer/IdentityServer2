using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class IdentityProvidersViewModel
    {
        private Repositories.IIdentityProviderRepository identityProviderRepository;

        public IdentityProvidersViewModel(Repositories.IIdentityProviderRepository identityProviderRepository)
        {
            // TODO: Complete member initialization
            this.identityProviderRepository = identityProviderRepository;
            this.IdentityProviders =
                identityProviderRepository.GetAll()
                .Select(x => new IPModel { DisplayName = x.DisplayName, Name = x.Name })
                .ToArray();
        }

        public IPModel[] IdentityProviders { get; set; }
    }

    public class IPModel 
    {
        public string DisplayName { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Delete { get; set; }
    }
}