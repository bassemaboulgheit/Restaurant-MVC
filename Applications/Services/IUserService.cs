using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.DTos;
using Microsoft.AspNetCore.Identity;
using Models;

namespace Applications.Services
{
    public interface IUserService
    {
        public  Task<IdentityResult> CreateAsync(RegisterDto user, string password);
        public  Task<ApplicationUser> FindByEmailAsync(string email);
        public  Task<ApplicationUser> FindByNameAsync(string name);
        public  Task<bool> IsEmailAvailableAsync(string email);
        public  Task<bool> CheckPasswordAsync(RegisterDto user, string password);
        public  Task<IdentityResult> AddToRoleAsync(RegisterDto user, string role);
        public  Task SignInAsync(RegisterDto user, bool isPersistent = false);
        public Task SignOutAsync();
    }
}
