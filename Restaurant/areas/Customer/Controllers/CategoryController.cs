using Applications.DTos;
using Applications.Services;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.areas.Customer.Controllers
{
    [Area(nameof(Customer))]
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

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyName(string name, int id)
        {
            var category = await categoryService.GetById(id);
            if (category != null && category.Name == name)
                return Json(true);

            if (!await categoryService.GetByName(name))
                return Json(true);

            return Json($"A category named {name} already exists.");
        }
    }
}
