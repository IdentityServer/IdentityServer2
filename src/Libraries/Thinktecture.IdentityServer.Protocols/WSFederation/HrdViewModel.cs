using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Protocols.WSFederation
{
    public class HrdViewModel
    {
        public HrdViewModel(System.IdentityModel.Services.SignInRequestMessage message, IEnumerable<Models.IdentityProvider> idps)
        {
            this.OriginalSigninUrl = message.WriteQueryString();
            this.Providers = idps.Select(x => new HRDIdentityProvider { DisplayName = x.DisplayName, ID = x.Name }).ToArray();
        }

        public IEnumerable<HRDIdentityProvider> Providers { get; set; }
        public string OriginalSigninUrl { get; set; }
        [Display(Name="Remember Selection")]
        public bool RememberHRDSelection { get; set; }
    }

    public class HRDIdentityProvider
    {
        public string ID { get; set; }
        public string DisplayName { get; set; }
    }
}
