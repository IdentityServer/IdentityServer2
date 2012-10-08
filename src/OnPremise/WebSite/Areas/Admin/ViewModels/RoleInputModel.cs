using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class RoleInputModel
    {
        [Required]
        public string Name { get; set; }

        [ScaffoldColumn(false)]
        public bool CanDelete
        {
            get
            {
                return Name != "IdentityServerAdministrators";
            }
        }
    }
}