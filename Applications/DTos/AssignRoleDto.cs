using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Applications.DTos
{
    public class AssignRoleDto
    {
        [Required(ErrorMessage = "Username or Email is required")]
        [Display(Name = "Username or Email")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Please select a role")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        public List<SelectListItem> Roles { get; set; } = new();
    }
}
