using Microsoft.AspNetCore.Mvc;
using Applications.DTos;
using Applications.Services;
using System.Threading.Tasks;
using Models;

namespace Restaurant.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto userDto)
        {
            if (ModelState.IsValid)
            {
                if (userDto.Password != userDto.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Password and confirmation password do not match.");
                    return View(userDto);
                }
                if (!await userService.IsEmailAvailableAsync(userDto.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    return View(userDto);
                }
                if (await userService.FindByNameAsync(userDto.userName) != null)
                {
                    ModelState.AddModelError("userName", "Username is already taken.");
                    return View(userDto);
                }
                await userService.CreateAsync(userDto);
                // Registration logic here
                return RedirectToAction("Login");
            }
            return View(userDto);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto userDto)
        {
            if(ModelState.IsValid)
            {
                // check
                var user = await userService.FindByNameAsync(userDto.userName);
                if (user != null)
                {
                    bool found = await userService.CheckPasswordAsync(userDto);
                    if(found == true)
                    {   
                        await userService.SignInAsync(userDto);
                        return RedirectToAction("GetAll","MenuItem");
                    }
                }
                ModelState.AddModelError("", "User Name Or Password Wrong");
            }
            return View("Login",userDto);
        }
        public async Task<IActionResult> SignOut()
        {
            await userService.SignOutAsync();
            return View("Login");
        }

    }
}
