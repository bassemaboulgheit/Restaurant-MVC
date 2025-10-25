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

        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = _cartService.GetCart(HttpContext.Session);
            if (cart.CartItems.Count == 0)
            {
                TempData["Error"] = "Your cart is empty";
                return RedirectToAction("GetAll", "MenuItem");
            }

            var createOrderDto = new CreateOrderDto
            {
                OrderDate = DateTime.Now,
                OrderType = OrderType.DineIn
            };

            return View(createOrderDto);
        }

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
                    return RedirectToAction("GetAll", "MenuItem");
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    TempData["Error"] = "User not found";
                    return RedirectToAction("Login", "Account");
                }

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

                var orderDto = await _orderService.CreateOrderAsync(createOrderDto, user.Id);
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

        [HttpGet]
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                var user = await _userManager.GetUserAsync(User);

                if (order.userId != user.Id)
                {
                    return Forbid();
                }

                ViewBag.Message = "Your order has been placed successfully!";
                return View(order);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToAction("GetAll", "MenuItem");
            }
        }

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

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                var user = await _userManager.GetUserAsync(User);

                if (order.userId != user.Id && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                // Customer Notifications
                ViewBag.StatusMessage = GetStatusNotification(order.OrderStatus);
                ViewBag.PrepTimeRemaining = GetPrepTimeRemaining(order);

                return View(order);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToAction("MyOrders");
            }
        }

        // Helper Methods
        private string GetStatusNotification(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => " Your order is pending. We'll start preparing it soon!",
                OrderStatus.Preparing => "Your order is being prepared. Please wait...",
                OrderStatus.Ready => " Your order is ready! Please pick it up or it will be delivered soon.",
                OrderStatus.Delivered => " Your order has been delivered. Thank you!",
                OrderStatus.Cancelled => " Your order has been cancelled.",
                _ => "Order status unknown"
            };
        }

        private string GetPrepTimeRemaining(OrderDto order)
        {
            if (order.LastUpdated == null || order.OrderStatus != OrderStatus.Preparing)
                return "";

            var timePassed = DateTime.Now - order.LastUpdated.Value;
            var timeRemaining = order.TotalPreparationTime - (int)timePassed.TotalMinutes;

            if (timeRemaining <= 0)
                return "Ready soon!";

            return $"{timeRemaining} minutes remaining";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var order = await _orderService.GetOrderByIdAsync(id);

                if (order == null || order.userId != user.Id)
                {
                    return Forbid();
                }

                var result = await _orderService.Cancel(id);

                if (!result)
                {
                    TempData["Error"] = "Cannot cancel this order.";
                }
                else
                {
                    TempData["Success"] = "Order cancelled successfully.";
                }

                return RedirectToAction("MyOrders");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToAction("MyOrders");
            }
        }

    }
}