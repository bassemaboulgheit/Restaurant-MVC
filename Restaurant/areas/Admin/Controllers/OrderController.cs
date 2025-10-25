using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Applications.Contracts;
using Applications.DTos.OrderDTOs;
using Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Restaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Admin: GET: Order/Index
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

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);

                if (order == null)
                {
                    TempData["Error"] = "Order not found";
                    return NotFound();
                }

                ViewBag.StatusNotification = GetStatusNotification(order.OrderStatus);

                return View(order);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // Admin: POST: Order/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, int status)
        {
            try
            {
                var orderStatus = (OrderStatus)status;
                var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, orderStatus);

                TempData["Success"] = $"Order #{id} status updated to {orderStatus}";

                var notification = GetStatusNotification(orderStatus);
                TempData["OrderNotification"] = notification;

                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error updating status: {ex.Message}";
                return RedirectToAction("Details", new { id });
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                TempData["Success"] = $"Order #{id} has been deleted successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Show specific error message
                if (ex.Message.Contains("Ready") || ex.Message.Contains("Delivered"))
                {
                    TempData["Error"] = "Cannot delete orders that are Ready or Delivered";
                }
                else
                {
                    TempData["Error"] = $"Error deleting order: {ex.Message}";
                }
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            if(!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid request";
                return RedirectToAction("Index");
            }
            else
            {
                var result =await _orderService.Cancel(id);
                if(!result)
                {
                    TempData["Error"] = "Cannot delete orders that are Ready or Delivered";
                    return RedirectToAction("Index");
                }
                TempData["Success"] = $"Order #{id} has been deleted successfully";
                return RedirectToAction("Index");
            }
        }

        private string GetStatusNotification(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => " Your order is pending. We'll start preparing it soon!",
                OrderStatus.Preparing => " Your order is being prepared. Please wait...",
                OrderStatus.Ready => " Your order is ready! Please pick it up or it will be delivered soon.",
                OrderStatus.Delivered => " Your order has been delivered. Thank you!",
                OrderStatus.Cancelled => "Your order has been cancelled.",
                _ => "Order status unknown"
            };
        }
    }
}