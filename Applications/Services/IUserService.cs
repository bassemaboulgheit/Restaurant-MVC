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
        public  Task<IdentityResult> CreateAsync(RegisterDto user);
        public  Task<ApplicationUser> FindByEmailAsync(string email);
        public  Task<ApplicationUser> FindByNameAsync(string name);
        public  Task<bool> IsEmailAvailableAsync(string email);
        public Task<bool> CheckPasswordAsync(LoginDto user);
        public  Task<IdentityResult> AddToRoleAsync(LoginDto user, string role);
        public  Task<bool> SignInAsync(LoginDto user);
        public Task SignOutAsync();
    }
}
