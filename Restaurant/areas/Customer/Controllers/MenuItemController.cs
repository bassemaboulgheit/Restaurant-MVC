using System.Threading.Tasks;
using Applications.DTos;
using Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Restaurant.areas.Customer.Controllers
{
    [Area(nameof(Customer))]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _itemService;
        private readonly ICategoryService _categoryService;

        public MenuItemController(IMenuItemService _itemService, ICategoryService _categoryService)
        {
            this._itemService = _itemService;
            this._categoryService = _categoryService;
        }
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemService.GetAll();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemService.GetById(id);
            return View(item);
        }

        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Message"] = "Please enter a valid item name.";
                return RedirectToAction(nameof(GetAll));
            }
            var items = await _itemService.GetListItemByName(name);

            if (items != null && items.Count > 0)
            {
                return View(nameof(GetAll), items);
            }
            TempData["Message"] = "No item found with that name.";
            return RedirectToAction(nameof(GetAll));
        }


    }
}
