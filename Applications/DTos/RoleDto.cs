using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.DTos
{
    public class RoleDto
    {
        [Display(Name ="Role Name")]
        public string roleName { get; set; }
    }
}
