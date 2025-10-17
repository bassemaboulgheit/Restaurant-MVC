using Microsoft.AspNetCore.Mvc;
using Applications.DTos;

namespace Restaurant.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {
            
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterDto userDto)
        {
            if (ModelState.IsValid)
            {

                // Registration logic here
                return RedirectToAction("Login");
            }
            return View(userDto);
        }
    }
}
