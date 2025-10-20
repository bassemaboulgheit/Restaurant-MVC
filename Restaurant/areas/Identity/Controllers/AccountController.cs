using System.Data;
using System.Threading.Tasks;
using Applications.DTos;
using Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Restaurant.areas.Identity.Controllers
{
    [Area(nameof(Identity))]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService _userService)
        {
            this._userService = _userService;
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

                var result = await _userService.CreateAsync(userDto);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Registration completed successfully. Please log in";
                    return RedirectToAction(nameof(Login));
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
                var result = await _userService.SignInAsync(userDto);

                if (result.Succeeded)
                {
                    var roles = await _userService.GetAllRolesAsync();
                    if (User.IsInRole(nameof(Admin)))
                        return RedirectToAction("GetAll", "MenuItem", new { area = nameof(Admin) });

                    else if (User.IsInRole(nameof(Customer)))
                        return RedirectToAction("GetAll", "MenuItem", new { area = nameof(Customer) });
                    else
                        return RedirectToAction("GetAll", "MenuItem", new { area = nameof(Identity) });

                    //return RedirectToAction("GetAll", "MenuItem");
                }

                if (result.IsLockedOut)
                    ModelState.AddModelError("", "Account locked. Try again later.");

                else
                ModelState.AddModelError("", "User Name Or Password Wrong");
            }
            return View("Login",userDto);
        }
        public async Task<IActionResult> SignOut()
        {
            await _userService.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
