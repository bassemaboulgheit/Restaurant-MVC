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
        public Guid Id { get; set; }

        [Required(ErrorMessage = " name is required")]
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
