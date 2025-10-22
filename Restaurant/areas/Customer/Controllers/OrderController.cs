using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Applications.Contracts;
using Applications.DTos.OrderDTOs;
using Applications.DTos.OrderItemsDTOs;
using Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Restaurant.Areas.Customer.Controllers
{
    [Area(nameof(Customer))]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(
            IOrderService orderService,
            ICartService cartService,
            UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _cartService = cartService;
            _userManager = userManager;
        }

        private async Task<string> GetCurrentUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id ?? "";
        }

        // GET: Order/Checkout
        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = _cartService.GetCart(HttpContext.Session);
            if (cart.CartItems.Count == 0)
            {
                TempData["Error"] = "Your cart is empty";
                return RedirectToAction("Index", "Menu");
            }

            var createOrderDto = new CreateOrderDto
            {
                OrderDate = DateTime.Now,
                OrderType = OrderType.DineIn
            };

            return View(createOrderDto);
        }

        // POST: Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CreateOrderDto createOrderDto)
        {
            try
            {
                var cart = _cartService.GetCart(HttpContext.Session);

                if (cart.CartItems.Count == 0)
                {
                    TempData["Error"] = "Your cart is empty";
                    return RedirectToAction("Index", "Menu");
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    TempData["Error"] = "User not found";
                    return RedirectToAction("Login", "Account");
                }

                // Map cart items to order items
                createOrderDto.OrderItems = cart.CartItems.Select(ci => new CreateOrderItemsDto
                {
                    MenuItemId = ci.MenuItemId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice,
                    Subtotal = ci.Total
                }).ToList();

                if (!ModelState.IsValid)
                {
                    return View(createOrderDto);
                }

                // Create order
                var orderDto = await _orderService.CreateOrderAsync(createOrderDto, user.Id);

                // Clear cart
                _cartService.ClearCart(HttpContext.Session);

                TempData["Success"] = "Order created successfully";
                return RedirectToAction("OrderConfirmation", new { id = orderDto.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return View(createOrderDto);
            }
        }

        // GET: Order/OrderConfirmation/5
        [HttpGet]
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                var user = await _userManager.GetUserAsync(User);

                // Check if user owns this order
                if (order.userId != user.Id)
                {
                    return Forbid();
                }

                return View(order);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToAction("Index", "Menu");
            }
        }

        // GET: Order/MyOrders
        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var orders = await _orderService.GetUserOrdersAsync(user.Id);
                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return View(new List<OrderDto>());
            }
        }

        // GET: Order/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                var user = await _userManager.GetUserAsync(User);

                // Check if user owns this order
                if (order.userId != user.Id && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                return View(order);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToAction("MyOrders");
            }
        }

        // Admin: GET: Order/Index
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return View(new List<OrderDto>());
            }
        }

        // Admin: POST: Order/UpdateStatus
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, int status)
        {
            try
            {
                var orderStatus = (OrderStatus)status;
                await _orderService.UpdateOrderStatusAsync(id, orderStatus);
                TempData["Success"] = "Order status updated successfully";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToAction("Details", new { id });
            }
        }

        // Admin: POST: Order/Delete
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                TempData["Success"] = "Order deleted successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}