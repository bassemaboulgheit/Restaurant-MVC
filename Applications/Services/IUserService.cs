using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Models;

namespace Applications.Services
{
    public interface IUserService
    {
        public  Task<ApplicationUser> FindByEmailAsync(string email);
        public  Task<ApplicationUser> FindByNameAsync(string name);
        public  Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        public  Task<bool> IsEmailAvailableAsync(string email);
        public  Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        public  Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        public  Task SignInAsync(ApplicationUser user, bool isPersistent = false);
        public Task SignOutAsync();
    }
}
