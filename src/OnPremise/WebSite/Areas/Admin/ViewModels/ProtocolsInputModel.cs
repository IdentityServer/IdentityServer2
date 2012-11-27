using System.ComponentModel.DataAnnotations;

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