using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.DTos.UsersDTo
{
    public class RegisterDto
    {
        //public Guid Id { get; set; }

        [Required(ErrorMessage = " userName is required")]
        [MaxLength(15, ErrorMessage = "userName cannot exceed 15 characters.")]
        [MinLength(3, ErrorMessage = "userName must be at least 3 characters long.")]
        [Display(Name = "User Name")]
        public string userName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(4, ErrorMessage = "Password must be at least 4 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string? Address { get; set; }
    }
}
