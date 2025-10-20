using System.Threading.Tasks;
using Applications.DTos;
using Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Restaurant.areas.Admin.Controllers
{
    [Authorize(Roles =nameof(Admin))]
    [Area(nameof(Admin))]
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

        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await _categoryService.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemsDto newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }
            await _itemService.Create(newItem);
            return RedirectToAction(nameof(GetAll));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _itemService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewBag.categories = await _categoryService.GetAll();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ItemsDto updatedItem)
        {
            if (ModelState.IsValid)
            {

                var item = await _itemService.GetById(updatedItem.Id);
                if (item == null)
                {
                    return NotFound();
                }
                await _itemService.Update(updatedItem);
                return RedirectToAction(nameof(GetAll));
            }
            ViewBag.categories = await _categoryService.GetAll();
            return View(updatedItem);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _itemService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            await _itemService.Delete(id);
            return RedirectToAction(nameof(GetAll));
        }
        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Message"] = "Please enter a valid item name.";
                return RedirectToAction(nameof(GetAll));
            }
            var item = await _itemService.GetItemByName(name);

            if (item != null)
            {
                return View("Details", item);
            }
            TempData["Message"] = "No item found with that name.";
            return RedirectToAction(nameof(GetAll));
        }

    }
}
