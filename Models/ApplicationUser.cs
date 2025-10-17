using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
        public List<Order>? Orders { get; set; } = new List<Order>();
    }
}
