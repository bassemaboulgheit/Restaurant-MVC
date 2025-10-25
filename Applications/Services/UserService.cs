using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Applications.DTos.AdminDTOs;
using Applications.DTos.UsersDTo;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Applications.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> CreateAsync(RegisterDto userDto)
        {
            if (userDto == null)
                return IdentityResult.Failed(new IdentityError { Description = "User data is null" });

            ApplicationUser? userApp = await _userManager.FindByEmailAsync(userDto.Email);
            if (userApp != null)
                return IdentityResult.Failed(new IdentityError { Description = "Email is already in use" });

            var userName = await _userManager.FindByNameAsync(userDto.userName);
            if (userName != null)
                return IdentityResult.Failed(new IdentityError { Description = "Username is already in use" });

            // Using Mapster for mapping
            var applicationUser = userDto.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(applicationUser, userDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(applicationUser, "Customer");
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                return IdentityResult.Success;
            }

            var allErrors = result.Errors.Select(e => new IdentityError { Description = e.Description }).ToArray();
            return IdentityResult.Failed(allErrors);
        }

        public async Task<SignInResult> SignInAsync(LoginDto userDto)
        {
            var user = await _userManager.FindByNameAsync(userDto.userName)
                ?? await _userManager.FindByEmailAsync(userDto.userName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, userDto.Password,
                    isPersistent: false, lockoutOnFailure: true);
                return result;
            }
            return SignInResult.Failed;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> CreateRoleAsync(RoleDto role)
        {
            bool exists = await _roleManager.RoleExistsAsync(role.roleName);
            if (exists)
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{role.roleName}' already exists" });

            var identityRole = new IdentityRole { Name = role.roleName };
            var result = await _roleManager.CreateAsync(identityRole);
            return result;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles;
        }

        public async Task<IdentityResult> AddToRoleAsync(LoginDto userDto, string roleName)
        {
            var user = await _userManager.FindByNameAsync(userDto.userName)
                ?? await _userManager.FindByEmailAsync(userDto.userName);

            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            if (!await _roleManager.RoleExistsAsync(roleName))
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{roleName}' does not exist" });

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result;
        }
    }
}