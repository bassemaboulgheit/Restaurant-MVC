using Applications.DTos.CategoryDTOs;
using Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles =nameof(Admin))]
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
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryDto newCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(newCategory);
            }
            await _categoryService.Create(newCategory);
            return RedirectToAction(nameof(GetAll));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryDto newCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(newCategory);
            }
            var category = await _categoryService.GetById(newCategory.Id);
            if (category == null)
            {
                return NotFound();
            }
            if (category.Name != newCategory.Name && await _categoryService.GetByName(newCategory.Name) != null)
            {
                ModelState.AddModelError("Name", "A category with this name already exists.");
                return View(newCategory);
            }
            await _categoryService.Update(newCategory);
            return RedirectToAction(nameof(GetAll));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            await _categoryService.Delete(id);
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
