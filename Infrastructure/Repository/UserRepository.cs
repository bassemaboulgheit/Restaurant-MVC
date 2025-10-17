using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }

        public async Task<ApplicationUser?> FindByIdAsync(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }
        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser?> FindByNameAsync(string name)
        {
            return await userManager.FindByNameAsync(name);
        }
        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user == null;
        }
        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await userManager.AddToRoleAsync(user, role);
        }
        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await userManager.GetRolesAsync(user);
        }
        public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            return await userManager.UpdateAsync(user);
        }
        
    }
}
