using System.Threading.Tasks;
using Applications.DTos;
using Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Restaurant.Controllers
{
    [Authorize]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService itemService;
        private readonly ICategoryService categoryService;

        public MenuItemController(IMenuItemService itemService, ICategoryService categoryService)
        {
            this.itemService = itemService;
            this.categoryService = categoryService;
        }
        public async Task<IActionResult> GetAll()
        {
            var items = await itemService.GetAll();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await itemService.GetById(id);
            return View(item);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await categoryService.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemsDto newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }
            await itemService.Create(newItem);
            return RedirectToAction("GetAll");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var item = await itemService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewBag.categories = await categoryService.GetAll();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ItemsDto updatedItem)
        {
            if (ModelState.IsValid)
            {

                var item = await itemService.GetById(updatedItem.Id);
                if (item == null)
                {
                    return NotFound();
                }
                await itemService.Update(updatedItem);
                return RedirectToAction("GetAll");
            }
            ViewBag.categories = await categoryService.GetAll();
            return View(updatedItem);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await itemService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            await itemService.Delete(id);
            return RedirectToAction("GetAll");
        }

    }
}
