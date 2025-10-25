//using System.Threading.Tasks;
//using Applications.DTos.ItemDTOs;
//using Applications.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Models;

//namespace Restaurant.areas.Admin.Controllers
//{
//    [Authorize(Roles =nameof(Admin))]
//    [Area(nameof(Admin))]
//    public class MenuItemController : Controller
//    {
//        private readonly IMenuItemService _itemService;
//        private readonly ICategoryService _categoryService;

//        public MenuItemController(IMenuItemService _itemService, ICategoryService _categoryService)
//        {
//            this._itemService = _itemService;
//            this._categoryService = _categoryService;
//        }
//        public async Task<IActionResult> GetAll()
//        {
//            var items = await _itemService.GetAll();
//            return View(items);
//        }

//        public async Task<IActionResult> Details(int id)
//        {
//            var item = await _itemService.GetById(id);
//            return View(item);
//        }

//        public async Task<IActionResult> Create()
//        {
//            ViewBag.categories = await _categoryService.GetAll();
//            return View(new CreateItemsDto());
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(CreateItemsDto newItem)
//        {
//            if (!ModelState.IsValid)
//            {
//                ViewBag.categories = await _categoryService.GetAll();
//                return View(newItem);
//            }

//            // الصوره
//            if (newItem.ImageFile != null && newItem.ImageFile.Length > 0)
//            {
//                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/MenuItems");
//                if (!Directory.Exists(uploadsFolder))
//                    Directory.CreateDirectory(uploadsFolder);

//                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(newItem.ImageFile.FileName);
//                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    await newItem.ImageFile.CopyToAsync(stream);
//                }

//                // حفظ المسار النسبي في قاعدة البيانات
//                newItem.ImageUrl = "/images/MenuItems/" + uniqueFileName;
//            }


//            await _itemService.Create(newItem);
//            return RedirectToAction(nameof(GetAll));
//        }
//        public async Task<IActionResult> Edit(int id)
//        {
//            var item = await _itemService.GetById(id);
//            if (item == null)
//            {
//                return NotFound();
//            }
//            ViewBag.categories = await _categoryService.GetAll();
//            return View(item);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Edit(ItemsDto updatedItem)
//        {
//            if (ModelState.IsValid)
//            {

//                var item = await _itemService.GetById(updatedItem.Id);
//                if (item == null)
//                {
//                    return NotFound();
//                }

//                //رفع الصورة الجديدة لو موجودة
//                if (updatedItem.ImageFile != null && updatedItem.ImageFile.Length > 0)
//                {
//                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/MenuItems");
//                    if (!Directory.Exists(uploadsFolder))
//                        Directory.CreateDirectory(uploadsFolder);

//                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(updatedItem.ImageFile.FileName);
//                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

//                    using (var stream = new FileStream(filePath, FileMode.Create))
//                    {
//                        await updatedItem.ImageFile.CopyToAsync(stream);
//                    }

//                    updatedItem.ImageUrl = "/images/MenuItems/" + uniqueFileName;
//                }
//                else
//                {
//                    // استخدم الصورة القديمة لو ما رفعش جديدة
//                    updatedItem.ImageUrl = item.ImageUrl;
//                }


//                await _itemService.Update(updatedItem);
//                return RedirectToAction(nameof(GetAll));
//            }
//            ViewBag.categories = await _categoryService.GetAll();
//            return View(updatedItem);
//        }
//        [HttpPost]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var item = await _itemService.GetById(id);
//            if (item == null)
//            {
//                return NotFound();
//            }
//            await _itemService.Delete(id);
//            return RedirectToAction(nameof(GetAll));
//        }

//        public async Task<IActionResult> Search(string name)
//        {
//            if (string.IsNullOrWhiteSpace(name))
//            {
//                TempData["Message"] = "Please enter a valid item name.";
//                return RedirectToAction(nameof(GetAll));
//            }
//            var items = await _itemService.GetListItemByName(name);

//            if (items != null && items.Count > 0)
//            {
//                return View(nameof(GetAll), items);
//            }
//            TempData["Message"] = "No item found with that name.";
//            return RedirectToAction(nameof(GetAll));
//        }
//    }
//}


using System.Threading.Tasks;
using Applications.DTos.ItemDTOs;
using Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.areas.Admin.Controllers
{
    [Authorize(Roles = nameof(Admin))]
    [Area(nameof(Admin))]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _itemService;
        private readonly ICategoryService _categoryService;

        public MenuItemController(IMenuItemService itemService, ICategoryService categoryService)
        {
            _itemService = itemService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> GetAll()
        {
            var items = await _itemService.GetAll();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemService.GetById(id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await _categoryService.GetAll();
            return View(new CreateItemsDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateItemsDto newItem)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.categories = await _categoryService.GetAll();
                return View(newItem);
            }

            // معالجة الصورة
            if (newItem.ImageFile != null && newItem.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/MenuItems");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(newItem.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await newItem.ImageFile.CopyToAsync(stream);
                }

                newItem.ImageUrl = "/images/MenuItems/" + uniqueFileName;
            }

            await _itemService.Create(newItem);
            TempData["SuccessMessage"] = "Item created successfully!";
            return RedirectToAction(nameof(GetAll));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _itemService.GetById(id);
            if (item == null)
                return NotFound();

            ViewBag.categories = await _categoryService.GetAll();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemsDto updatedItem)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.categories = await _categoryService.GetAll();
                return View(updatedItem);
            }

            var item = await _itemService.GetById(updatedItem.Id);
            if (item == null)
                return NotFound();

            // رفع الصورة الجديدة لو موجودة
            if (updatedItem.ImageFile != null && updatedItem.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/MenuItems");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(updatedItem.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updatedItem.ImageFile.CopyToAsync(stream);
                }

                updatedItem.ImageUrl = "/images/MenuItems/" + uniqueFileName;
            }
            else
            {
                // استخدم الصورة القديمة
                updatedItem.ImageUrl = item.ImageUrl;
            }

            await _itemService.Update(updatedItem);
            TempData["SuccessMessage"] = "Item updated successfully!";
            return RedirectToAction(nameof(GetAll));
        }

        // Delete via AJAX (من SweetAlert)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDirect(int id)
        {
            var result = await _itemService.Delete(id);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction(nameof(GetAll));
        }

        // API endpoint للتحقق من Orders (للـ AJAX)
        [HttpGet]
        public async Task<IActionResult> CheckItemOrders(int id)
        {
            bool hasOrders = await _itemService.HasOrders(id);
            return Json(new { hasOrders });
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