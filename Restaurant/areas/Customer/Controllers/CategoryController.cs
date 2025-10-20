using Applications.DTos;
using Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.areas.Customer.Controllers
{
    [Area(nameof(Customer))]
    //[Authorize(Roles = nameof(Customer))]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMenuItemService _menuItemService;

        public CategoryController(ICategoryService _categoryService, IMenuItemService _menuItemService)
        {
            this._categoryService = _categoryService;
            this._menuItemService = _menuItemService;
        }
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAll();
            return View(categories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.item = await _menuItemService.GetAll();
            return View(category);
        }
        public async Task<IActionResult> Search(string name)
        {
            var category = await _categoryService.GetCategoryByName(name);

            if (category != null)
            {
                return View(nameof(Details), category);
            }

            TempData["NotFoundMessage"] = $"No category found with name '{name}'.";
            return RedirectToAction(nameof(GetAll));
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyName(string name, int id)
        {
            var category = await _categoryService.GetById(id);
            if (category != null && category.Name == name)
                return Json(true);

            if (!await _categoryService.GetByName(name))
                return Json(true);

            return Json($"A category named {name} already exists.");
        }
    }
}
