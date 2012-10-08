using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Thinktecture.IdentityServer.Models.Configuration;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class ProtocolsInputModel
    {
        [Required]
        public bool[] Protocols { get; set; }

        public void Update(Repositories.IConfigurationRepository configurationRepository)
        {
            new ProtocolsViewModel(configurationRepository).Update(this.Protocols);
        }
    }
}