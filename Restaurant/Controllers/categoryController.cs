using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using Applications.DTos;
using Applications.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Restaurant.Controllers
{
    public class categoryController : Controller
    {
        private readonly IMenuCategoryService categoryService;
        private readonly IMenuItemService menuItemService;

        public categoryController(IMenuCategoryService categoryService, IMenuItemService menuItemService)
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
            return View("GetAll",categories);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto newCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(newCategory);
            }
            //if (await categoryService.GetByName(newCategory.Name) != null)
            //{
            //    ModelState.AddModelError("Name", "A category with this name already found.");
            //    return View(newCategory);
            //}
            await categoryService.Create(newCategory);
            return RedirectToAction("GetAll");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryDto newCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(newCategory);
            }
            var category = await categoryService.GetById(newCategory.Id);
            if (category == null)
            {
                return NotFound();
            }
            if (category.Name != newCategory.Name && await categoryService.GetByName(newCategory.Name) != null)
            {
                ModelState.AddModelError("Name", "A category with this name already exists.");
                return View(newCategory);
            }
            await categoryService.Update(newCategory);
            return RedirectToAction("GetAll");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var category = await categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            await categoryService.Delete(id);
            return RedirectToAction("GetAll");
        }


        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyName(string name,int id)
        {
            var category = await categoryService.GetById(id);
            if (category != null && category.Name == name)
             return Json(true);

            if (!await categoryService.GetByName1(name))
                return Json(true);

            return Json($"A category named {name} already exists.");
        }
    }
}
