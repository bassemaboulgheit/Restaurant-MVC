using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.DTos;
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

        public async Task<IdentityResult> CreateAsync(RegisterDto user, string password)
        {
            if (user != null)
            {
                var existingUser = await userRepo.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    throw new Exception("Email is already in use");
                }
                var ApplicationUser = new ApplicationUser
                {
                    UserName = user.userName,
                    PasswordHash = password,
                    Email = user.Email,
                    Address = user.Address
                };
                var result = await userRepo.CreateAsync(ApplicationUser, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create user: {errors}");
                }
                return result;
            }
            
            throw new Exception("User data is null");
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
        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }
            var result = await userRepo.IsEmailAvailableAsync(email);
            return result;
        }

        public async Task<bool> CheckPasswordAsync(RegisterDto user, string password)
        {
            if (user != null)
            {
                var existingUser = await userRepo.FindByEmailAsync(user.Email);
                if (existingUser == null)
                {
                    var ApplicationUser = new ApplicationUser
                    {
                        UserName = user.userName,
                        PasswordHash = password,
                        Email = user.Email,
                        Address = user.Address
                    };
                    var result = await userRepo.CheckPasswordAsync(existingUser, password);
                    return result;
                }
                throw new Exception("User not found");
            }
            throw new Exception("User data is null");
        }
        public async Task<IdentityResult> AddToRoleAsync(RegisterDto user, string role)
        {
            if (user != null)
            {
                var existingUser = await userRepo.FindByEmailAsync(user.Email);
                if (existingUser == null)
                {
                    throw new Exception("User not found");
                }
                var ApplicationUser = new ApplicationUser
                {
                    UserName = user.userName,
                    PasswordHash = user.Password,
                    Email = user.Email,
                    Address = user.Address
                };
                var result = await userRepo.AddToRoleAsync(ApplicationUser, role);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to add user to role: {errors}");
                }
                return result;
            }
            throw new Exception("User data is null");
        }
        public async Task SignInAsync(RegisterDto userDto, bool isPersistent = false)
        {
            var user = await userRepo.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var result = await userRepo.CheckPasswordAsync(user, userDto.Password);
            if (!result)
            {
                throw new Exception("Invalid password");
            }
            var ApplicationUser = new ApplicationUser
            {
                UserName = userDto.userName,
                PasswordHash = userDto.Password,
                Email = userDto.Email,
                Address = userDto.Address
            };
            await signInManager.SignInAsync(ApplicationUser, isPersistent);
        }
        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }
        
    }
}
