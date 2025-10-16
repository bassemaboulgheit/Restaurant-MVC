using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.DTos
{
    public class RegisterDto
    {
        //public Guid Id { get; set; }

        [Required(ErrorMessage = " name is required")]
        public string Name { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        public string Role { get; set; }


        public string? Address { get; set; }
    }
}
