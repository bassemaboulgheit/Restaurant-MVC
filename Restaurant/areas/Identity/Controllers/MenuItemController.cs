using System.Threading.Tasks;
using Applications.DTos;
using Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Restaurant.areas.Identity.Controllers
{
    //[Authorize]
    [Area(nameof(Identity))]
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
    }
}
