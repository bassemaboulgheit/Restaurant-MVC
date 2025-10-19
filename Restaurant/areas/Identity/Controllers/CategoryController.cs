using Applications.DTos;
using Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.areas.Identity.Controllers
{
    [Area(nameof(Identity))]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IMenuItemService menuItemService;

        public CategoryController(ICategoryService categoryService, IMenuItemService menuItemService)
        {
            this.categoryService = categoryService;
            this.menuItemService = menuItemService;
        }
        public async Task<IActionResult> GetAll()
        {
            var categories = await categoryService.GetAll();
            return View(categories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.item = await menuItemService.GetAll();
            return View(category);
        }
        public async Task<IActionResult> Search(string name)
        {
            var categories = await categoryService.GetByName(name);
            return View("GetAll", categories);
        }
    }
}
