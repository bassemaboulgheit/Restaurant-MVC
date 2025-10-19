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
        public  Task<IdentityResult> CreateAsync(RegisterDto userDto);
        public  Task<SignInResult> SignInAsync(LoginDto userDto);
        public  Task SignOutAsync();

        //public Task<SignInResult> ResetPasswordAsync();
        public Task<IdentityResult> CreateRoleAsync(RoleDto role);
        public Task<List<IdentityRole>> GetAllRolesAsync();
        public Task<IdentityResult> AddToRoleAsync(LoginDto userDto, string roleName);
    }
}
