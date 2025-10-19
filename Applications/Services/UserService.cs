using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Applications.DTos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Applications.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager , RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateAsync(RegisterDto userDto)
        {
            if (userDto == null)
                return IdentityResult.Failed(new IdentityError { Description = "User data is null" });

            ApplicationUser? userApp = await userManager.FindByEmailAsync(userDto.Email);
            if (userApp != null)
                return IdentityResult.Failed(new IdentityError { Description = "Email is already in use" });

            var userName = await userManager.FindByNameAsync(userDto.userName);
            if (userName != null)
                return IdentityResult.Failed(new IdentityError { Description = "Username is already in use" });
            
            //mapping DTO to ApplicationUser
            var applicationUser = new ApplicationUser
            {
                UserName = userDto.userName,
                Email = userDto.Email,
                Address = userDto.Address
            };

            // save user to database
            var result = await userManager.CreateAsync(applicationUser, userDto.Password);
            if (result.Succeeded)
            {
                //var role = await userManager.AddToRoleAsync( ApplicationUser,"Customer" );

                await signInManager.SignInAsync(applicationUser, isPersistent: false);
                return IdentityResult.Success;
            }

            var allErrors = result.Errors.Select(e => new IdentityError { Description = e.Description }).ToArray();
            return IdentityResult.Failed(allErrors);

        }

        public async Task<SignInResult> SignInAsync(LoginDto userDto)
        {
            var user = await userManager.FindByNameAsync(userDto.userName)
                ?? await userManager.FindByEmailAsync(userDto.userName) ;

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, userDto.Password,
                    isPersistent: false, lockoutOnFailure: true);
                return result;
            }
            return SignInResult.Failed;
        }

        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> CreateRoleAsync(RoleDto role)
        {
            bool exists = await roleManager.RoleExistsAsync(role.roleName);
            if (exists)
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{role.roleName}' already exists"});
            
            var identityRole = new IdentityRole
            {
                Name = role.roleName,
            };
            var result = await roleManager.CreateAsync(identityRole);
            return result;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return roles;
        }

        public async Task<IdentityResult> AddToRoleAsync(LoginDto userDto, string roleName)
        {
            var user = await userManager.FindByNameAsync(userDto.userName)
                ?? await userManager.FindByEmailAsync(userDto.userName);

            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            if (!await roleManager.RoleExistsAsync(roleName))
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{roleName}' does not exist" });

            var result = await userManager.AddToRoleAsync(user, roleName);
            return result;
        }













        //public async Task<IdentityResult> CreateAsync(RegisterDto user)
        //{
        //    if (user != null)
        //    {
        //        //var existingUser = await userRepo.FindByEmailAsync(user.Email);
        //        var existingUser = await userManager.FindByEmailAsync(user.Email);
        //        if (existingUser != null)
        //        {
        //            //throw new Exception("Email is already in use");
        //            return IdentityResult.Failed(new IdentityError { Description = "Email is already in use" });
        //        }
        //        //var existingUserName = await userRepo.FindByNameAsync(user.userName);
        //        var existingUserName = await userManager.FindByNameAsync(user.userName);
        //        if (existingUserName != null)
        //        {
        //            //throw new Exception("Email is already in use");
        //            return IdentityResult.Failed(new IdentityError { Description = "Username is already in use" });
        //        }
        //        //mapping DTO to ApplicationUser
        //        var ApplicationUser = new ApplicationUser();
        //        {
        //            ApplicationUser.UserName = user.userName;
        //            ApplicationUser.Email = user.Email;
        //            ApplicationUser.Address = user.Address;
        //        };
        //        // save user to database
        //        //IdentityResult result = await userRepo.CreateAsync(ApplicationUser,user.Password);
        //        IdentityResult result = await userManager.CreateAsync(ApplicationUser,user.Password);
        //        if (result.Succeeded)
        //        {
        //            //await userRepo.AddToRoleAsync(ApplicationUser, "Test");
        //            //await userManager.AddToRoleAsync(ApplicationUser, "Test");
        //            var claims = new List<Claim>();
        //            //{
        //                //new Claim(ClaimTypes.Name,user.userName),
        //                //new Claim(ClaimTypes.Email,user.Email),
        //                //new Claim("Address",user.Address?? ""),
        //                //new Claim(ClaimTypes.Role,"User"),
        //            //};
        //            //await userRepo.AddClaimsAsync(ApplicationUser, claims);
        //            await userManager.AddClaimsAsync(ApplicationUser, claims);

        //            //create cookie
        //            await signInManager.SignInAsync(ApplicationUser, isPersistent: false);
        //            return result;
        //        }
        //        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        //        throw new Exception($"Failed to create user: {errors}");
        //    }
        //    throw new Exception("User data is null");
        //}


        //public async Task<bool> SignInAsync(LoginDto userDto)
        //{
        //    //var user = await userRepo.FindByNameAsync(userDto.userName);
        //    var user = await userManager.FindByNameAsync(userDto.userName);
        //    if (user != null)
        //    {
        //        //var result = await userRepo.CheckPasswordAsync(user, userDto.Password);
        //        //var result = await userManager.CheckPasswordAsync(user, userDto.Password);
        //        //if (result == true)
        //        //{
        //        //    //await signInManager.SignInAsync(user, isPersistent : userDto.RememberMe);
        //        //    List<Claim> claims = new List<Claim>();
        //        //    claims.Add(new Claim("UserEmail", userApp.Email));
        //        //    await signInManager.SignInWithClaimsAsync(user, isPersistent : userDto.RememberMe,claims);
        //        //    return true;
        //        //}
        //        bool found = await userManager.CheckPasswordAsync(user, userDto.Password);
        //        if (found != null)
        //        {
        //            // create cookie
        //            List<Claim> claims = new List<Claim>();
        //            //claims.Add(new Claim("UserEmail", user.Address));
        //            await signInManager.SignInWithClaimsAsync(user, userDto.RememberMe, claims);
        //        }
        //    }
        //    return false;
        //}

        //public async Task<IList<string>> GetUserRolesAsync(LoginDto userDto)
        //{
        //    var user = await userRepo.FindByNameAsync(userDto.userName);
        //    if (user == null)
        //        throw new Exception("User not found");

        //    return await userRepo.GetRolesAsync(user);
        //}
        //public async Task<IdentityResult> CreateRoleAsync(RoleDto roleName)
        //{
        //    if (roleName == null || string.IsNullOrWhiteSpace(roleName.roleName))
        //    {
        //        throw new Exception("Role name is required");
        //    }
        //    var role = new IdentityRole
        //    {
        //        Name = roleName.roleName
        //    };
        //    return await userRepo.AddRoleAsync(role);
        //}


        //public async Task<IdentityResult> AddToRoleAsync(LoginDto userName, string role)
        //{
        //    if (userName != null)
        //    {
        //        //var existingUser = await userRepo.FindByNameAsync(userName.userName);
        //        var existingUser = await userManager.FindByNameAsync(userName.userName);
        //        if (existingUser == null)
        //        {
        //            throw new Exception("User not found");
        //        }
        //        var existingRole = await roleManager.RoleExistsAsync(role);
        //        if (existingRole == true)
        //        {
        //            return await roleManager.CreateAsync(new IdentityRole { Name = role });
        //        }

        //        var result = await userRepo.AddToRoleAsync(existingUser, role);
        //        if (!result.Succeeded)
        //        {
        //            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        //            throw new Exception($"Failed to add user to role: {errors}");
        //        }
        //        return result;
        //    }
        //    throw new Exception("User data is null");
        //}

        //public async Task<bool> CheckPasswordAsync(LoginDto userDto)
        //{
        //    if (userDto != null)
        //    {
        //        //ApplicationUser? user = await userRepo.FindByNameAsync(userDto.userName);
        //        ApplicationUser? user = await userManager.FindByNameAsync(userDto.userName);
        //        if (user != null)
        //        {
        //            //return await userRepo.CheckPasswordAsync(user, userDto.Password);
        //            return await userManager.CheckPasswordAsync(user, userDto.Password);
        //        }
        //        return false;
        //    }
        //    return false;
        //}

        //public async Task<bool> IsEmailAvailableAsync(string email)
        //{
        //    if(email != null)
        //    {
        //        //return await userRepo.IsEmailAvailableAsync(email);
        //        var user = await userManager.FindByEmailAsync(email);
        //        if (user != null)
        //            return false;
        //        return true;
        //    }
        //    return false;
        //}

        //public async Task<ApplicationUser?> FindByNameAsync(string name)
        //{
        //    //return await userRepo.FindByNameAsync(name);
        //    return await userManager.FindByNameAsync(name);
        //    //var user = await userRepo.FindByNameAsync(name);
        //    //if (user == null)
        //    //{
        //    //    throw new Exception("User not found");
        //    //}
        //    //return user;
        //}

        //public async Task<ApplicationUser?> FindByEmailAsync(string email)
        //{
        //    //return await userRepo.FindByEmailAsync(email);
        //    //var user = await userRepo.FindByEmailAsync(email);
        //    var user = await userManager.FindByEmailAsync(email);
        //    if (user != null)
        //    {
        //        return user;
        //    }
        //    throw new Exception("User not found");
        //}
    }
}
