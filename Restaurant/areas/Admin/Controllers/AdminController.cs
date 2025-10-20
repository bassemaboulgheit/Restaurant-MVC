using System.Threading.Tasks;
using Applications.DTos;
using Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area(nameof(Admin))]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(IUserService _userService , RoleManager<IdentityRole> _roleManager)
        {
            this._userService = _userService;
            this._roleManager = _roleManager;
        }
        [HttpGet]
        [Authorize]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(RoleDto roleDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateRoleAsync(roleDto);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = $"Role '{roleDto.roleName}' created successfully.";
                    return RedirectToAction(nameof(GetAllRoles));
                }
                foreach(var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(nameof(AddRole), roleDto);
        }

        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _userService.GetAllRolesAsync();
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole()
        {
            var roles = await _roleManager.Roles
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToListAsync();

            var model = new AssignRoleDto
            {
                Roles = roles
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignRoleDto model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = await _roleManager.Roles
                    .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                    .ToListAsync();
                return View(model);
            }

            var loginDto = new LoginDto { userName = model.UserNameOrEmail };
            var result = await _userService.AddToRoleAsync(loginDto, model.RoleName);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"User '{model.UserNameOrEmail}' added to role '{model.RoleName}' successfully.";
                return RedirectToAction(nameof(AssignRole));
            }

            TempData["ErrorMessage"] = string.Join(" | ", result.Errors.Select(e => e.Description));
            model.Roles = await _roleManager.Roles
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToListAsync();

            return View(model);
        }
    }
}
