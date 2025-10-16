using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Identity;
using Models;

namespace Applications.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepo;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserService(IUserRepository userRepo , SignInManager<ApplicationUser> signInManager)
        {
            this.userRepo = userRepo;
            this.signInManager = signInManager;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var user = await userRepo.FindByEmailAsync(email);
            if (user == null) 
                {
                throw new Exception("User not found");
            }
            return user;
        }
        public async Task<ApplicationUser> FindByNameAsync(string name)
        {
            var user = await userRepo.FindByNameAsync(name);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            var result = await userRepo.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
            }
            return result;
        }

        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            return await userRepo.IsEmailAvailableAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await userRepo.CheckPasswordAsync(user, password);
        }
        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            var result = await userRepo.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to add user to role: {errors}");
            }
            return result;
        }
        public async Task SignInAsync(ApplicationUser user, bool isPersistent = false)
        {
            await signInManager.SignInAsync(user, isPersistent);
        }
        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }
        
    }
}
