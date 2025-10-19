using System.Data;
using System.Threading.Tasks;
using Applications.DTos;
using Applications.Services;
using Microsoft.AspNetCore.Mvc;
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

                var result = await userService.CreateAsync(userDto);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Registration completed successfully. Please log in";
                    return RedirectToAction("Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
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
            if (ModelState.IsValid)
            {
                var result = await userService.SignInAsync(userDto);

                if (result.Succeeded)
                 return RedirectToAction("GetAll", "MenuItem");

                if (result.IsLockedOut)
                    ModelState.AddModelError("", "Account locked. Try again later.");

                else
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
