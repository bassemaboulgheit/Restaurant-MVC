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

        public async Task<IdentityResult> CreateAsync(RegisterDto user)
        {
            if (user != null)
            {
                var existingUser = await userRepo.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    throw new Exception("Email is already in use");
                }
                //mapping DTO to ApplicationUser
                var ApplicationUser = new ApplicationUser
                {
                    UserName = user.userName,
                    //PasswordHash = user.Password,
                    Email = user.Email,
                    Address = user.Address
                };
                // save user to database
                IdentityResult result = await userRepo.CreateAsync(ApplicationUser,user.Password);
                if (result.Succeeded)
                {
                    //create cookie
                    await signInManager.SignInAsync(ApplicationUser, isPersistent: false);
                    return result;
                }
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
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
        public async Task<bool> CheckPasswordAsync(LoginDto userDto)
        {
            if (userDto != null)
            {
                ApplicationUser? user = await userRepo.FindByNameAsync(userDto.userName);
                if (user != null)
                {
                    var result = await userRepo.CheckPasswordAsync(user, userDto.Password);
                    return result;
                }
                throw new Exception("User not found");
            }
            return false;
        }
        public async Task<IdentityResult> AddToRoleAsync(LoginDto userDto, string role) 
        {
            if (userDto != null)
            {
                var existingUser = await userRepo.FindByNameAsync(userDto.userName);
                if (existingUser == null)
                {
                    throw new Exception("User not found");
                }
                var result = await userRepo.AddToRoleAsync(existingUser, role);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to add user to role: {errors}");
                }
                return result;
            }
            throw new Exception("User data is null");
        }
        public async Task<bool> SignInAsync(LoginDto userDto)
        {
            var user = await userRepo.FindByNameAsync(userDto.userName);
            if (user != null)
            {
                var result = await userRepo.CheckPasswordAsync(user, userDto.Password);
                if (result)
                {
                    await signInManager.SignInAsync(user,userDto.RememberMe);
                    return true;
                }
            }
            return false;
        }
        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }
        
    }
}
