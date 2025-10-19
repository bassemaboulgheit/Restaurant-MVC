using System.Threading.Tasks;
using Applications.DTos;
using Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.areas.Identity.Controllers
{
    //[Authorize(Roles ="Admin")]
    [Area(nameof(Identity))]
    public class AdminController : Controller
    {
        private readonly IUserService userService;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminController(IUserService userService , RoleManager<IdentityRole> roleManager)
        {
            this.userService = userService;
            this.roleManager = roleManager;
        }
        [HttpGet]
        [Authorize]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddRole(RoleDto roleDto)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.CreateRoleAsync(roleDto);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = $"Role '{roleDto.roleName}' created successfully.";
                    return RedirectToAction("GetAllRoles");
                }
                foreach(var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View("AddRole", roleDto);
        }

        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await userService.GetAllRolesAsync();
            return View(roles);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole()
        {
            var roles = await roleManager.Roles
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToListAsync();

            var model = new AssignRoleDto
            {
                Roles = roles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRoleDto model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = await roleManager.Roles
                    .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                    .ToListAsync();
                return View(model);
            }

            var loginDto = new LoginDto { userName = model.UserNameOrEmail };
            var result = await userService.AddToRoleAsync(loginDto, model.RoleName);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"User '{model.UserNameOrEmail}' added to role '{model.RoleName}' successfully.";
                return RedirectToAction("AssignRole");
            }

            TempData["ErrorMessage"] = string.Join(" | ", result.Errors.Select(e => e.Description));
            model.Roles = await roleManager.Roles
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToListAsync();

            return View(model);
        }
    }
}
