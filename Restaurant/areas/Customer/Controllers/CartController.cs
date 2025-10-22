using Applications.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Restaurant.Areas.Customer.Controllers
{
    //[Area(nameof(Customer))]
    //public class CartController : Controller
    //{
    //    private readonly ICartService _cartService;

    //    public CartController(ICartService cartService)
    //    {
    //        _cartService = cartService;
    //    }

    //    public IActionResult Index()
    //    {
    //        var cart = _cartService.GetCart(HttpContext.Session);
    //        return View(cart);
    //    }

    //    public IActionResult Add(int id, string name, decimal price)
    //    {
    //        var item = new CartItem
    //        {
    //            MenuItemId = id,
    //            Name = name,
    //            UnitPrice = price,
    //            Quantity = 1
    //        };

    //        _cartService.AddItem(HttpContext.Session, item);
    //        return RedirectToAction("Index");
    //    }

    //    public IActionResult Remove(int id)
    //    {
    //        _cartService.RemoveItem(HttpContext.Session, id);
    //        return RedirectToAction("Index");
    //    }

    //    public IActionResult Clear()
    //    {
    //        _cartService.ClearCart(HttpContext.Session);
    //        return RedirectToAction("Index");
    //    }
    //}

    using Applications.Contracts;
    using Applications.DTos.ItemDTOs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    namespace Restaurant.Areas.Customer.Controllers
    {
        [Authorize]
        [Area(nameof(Customer))]
        public class CartController : Controller
        {
            private readonly ICartService _cartService;
            private readonly IGenericRepository<MenuItem> _menuItemRepository;

            public CartController(
                ICartService cartService,
                IGenericRepository<MenuItem> menuItemRepository)
            {
                _cartService = cartService;
                _menuItemRepository = menuItemRepository;
            }

            // POST: Cart/AddToCart
            [HttpPost]
            public async Task<IActionResult> AddToCart(int menuItemId, int quantity)
            {
                try
                {
                    var menuItem = await _menuItemRepository.GetById(menuItemId);

                    if (menuItem == null)
                    {
                        TempData["Error"] = "Item not found";
                        return RedirectToAction("GetAll", "MenuItem");
                    }

                    if (quantity <= 0 || quantity > menuItem.Quantity)
                    {
                        TempData["Error"] = "Invalid quantity";
                        return RedirectToAction("Details", "MenuItem", new { id = menuItemId });
                    }

                    var cartItem = new CartItem
                    {
                        MenuItemId = menuItem.Id,
                        Name = menuItem.Name,
                        UnitPrice = menuItem.Price,
                        Quantity = quantity,
                        ImageUrl = menuItem.ImageUrl
                    };

                    _cartService.AddItem(HttpContext.Session, cartItem);
                    TempData["Success"] = $"{menuItem.Name} added to cart";

                    return RedirectToAction("ViewCart");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error: {ex.Message}";
                    return RedirectToAction("GetAll", "MenuItem");
                }
            }

            // GET: Cart/ViewCart
            [HttpGet]
            public IActionResult ViewCart()
            {
                var cart = _cartService.GetCart(HttpContext.Session);
                return View(cart);
            }

            // POST: Cart/RemoveItem
            [HttpPost]
            public IActionResult RemoveItem(int menuItemId)
            {
                try
                {
                    _cartService.RemoveItem(HttpContext.Session, menuItemId);
                    TempData["Success"] = "Item removed from cart";
                    return RedirectToAction("ViewCart");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error: {ex.Message}";
                    return RedirectToAction("ViewCart");
                }
            }

            // POST: Cart/ClearCart
            [HttpPost]
            public IActionResult ClearCart()
            {
                try
                {
                    _cartService.ClearCart(HttpContext.Session);
                    TempData["Success"] = "Cart cleared";
                    return RedirectToAction("GetAll", "MenuItem");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error: {ex.Message}";
                    return RedirectToAction("ViewCart");
                }
            }

            // GET: Cart/GetTotalPrice
            [HttpGet]
            public decimal GetTotalPrice()
            {
                return _cartService.GetTotalPrice(HttpContext.Session);
            }
        }
    }
}