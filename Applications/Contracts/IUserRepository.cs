using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Models;

namespace Infrastructure.Contracts
{
    public interface IUserRepository
    {
        public Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        public Task<ApplicationUser?> FindByEmailAsync(string email);
        public Task<ApplicationUser?> FindByNameAsync(string name);
        //public Task<ApplicationUser?> FindByIdAsync(string userId);
        public Task<bool> IsEmailAvailableAsync(string email);
        public Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        //public Task<IList<string>> GetRolesAsync(ApplicationUser user);
        public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        public Task<IdentityResult> UpdateAsync(ApplicationUser user);
    }
}
