using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class UserInputModel
    {


        [Required]
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public UserRoleAssignment[] Roles { get; set; }
    }
}