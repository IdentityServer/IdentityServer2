using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class UserPasswordModel
    {
        [Required]
        [UIHint("HiddenInput")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}