using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
