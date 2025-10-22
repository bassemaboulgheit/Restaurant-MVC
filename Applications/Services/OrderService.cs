using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Applications.Contracts;
using Applications.DTos.ItemDTOs;
using Applications.DTos.OrderDTOs;
using Applications.DTos.OrderItemsDTOs;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Applications.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderItem> _orderItemRepository;
        private readonly IGenericRepository<MenuItem> _menuItemRepository;

        public OrderService(
            IGenericRepository<Order> orderRepository,
            IGenericRepository<OrderItem> orderItemRepository,
            IGenericRepository<MenuItem> menuItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _menuItemRepository = menuItemRepository;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto, string userId)
        {
            // Calculate subtotal from order items
            decimal subtotal = 0;
            var orderItems = new List<OrderItem>();

            foreach (var itemDto in createOrderDto.OrderItems)
            {
                var menuItem = await _menuItemRepository.GetById(itemDto.MenuItemId);
                if (menuItem == null)
                    throw new Exception($"Menu item with ID {itemDto.MenuItemId} not found");

                var orderItem = new OrderItem
                {
                    MenuItemId = itemDto.MenuItemId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    Subtotal = itemDto.Quantity * itemDto.UnitPrice
                };

                subtotal += orderItem.Subtotal;
                orderItems.Add(orderItem);
            }

            // Calculate discount
            decimal discountAmount = CalculateDiscount(subtotal, createOrderDto.OrderDate);

            // Calculate tax
            decimal taxAmount = CalculateTax(subtotal, discountAmount);

            // Calculate total
            decimal totalAmount = CalculateTotal(subtotal, discountAmount, taxAmount);

            // Create order
            var order = new Order
            {
                CustomerName = createOrderDto.CustomerName,
                CustomerPhone = createOrderDto.CustomerPhone,
                CustomerEmail = createOrderDto.CustomerEmail,
                OrderType = createOrderDto.OrderType,
                OrderStatus = OrderStatus.Pending,
                OrderDate = DateTime.Now,
                DeliveryAddress = createOrderDto.DeliveryAddress ?? "",
                ApplicationUserId = userId,
                Subtotal = subtotal,
                DiscountAmount = discountAmount,
                TaxAmount = taxAmount,
                TotalAmount = totalAmount,
                OrderItems = orderItems,
                LastUpdated = DateTime.Now
            };

            if (createOrderDto.OrderType == OrderType.Delivery)
            {
                order.EstimatedDeliveryTime = CalculateEstimatedDeliveryTime(orderItems);
            }

            await _orderRepository.Create(order);
            await _orderRepository.Save();

            return MapOrderToDto(order);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetById(id,
                o => o.OrderItems,
                o => o.ApplicationUser);

            if (order == null)
                throw new Exception("Order not found");

            return MapOrderToDto(order);
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAll(
                o => o.OrderItems,
                o => o.ApplicationUser);

            return orders.Select(o => MapOrderToDto(o)).ToList();
        }

        public async Task<List<OrderDto>> GetUserOrdersAsync(string userId)
        {
            var orders = await _orderRepository.GetAll(
                o => o.OrderItems,
                o => o.ApplicationUser);

            var userOrders = orders.Where(o => o.ApplicationUserId == userId).ToList();
            return userOrders.Select(o => MapOrderToDto(o)).ToList();
        }

        public async Task<OrderDto> UpdateOrderAsync(int id, OrderDto orderDto)
        {
            var order = await _orderRepository.GetById(id, o => o.OrderItems);

            if (order == null)
                throw new Exception("Order not found");

            order.CustomerName = orderDto.CustomerName;
            order.CustomerPhone = orderDto.CustomerPhone;
            order.CustomerEmail = orderDto.CustomerEmail;
            order.DeliveryAddress = orderDto.DeliveryAddress ?? "";
            order.LastUpdated = DateTime.Now;

            await _orderRepository.Update(order);
            await _orderRepository.Save();

            return MapOrderToDto(order);
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(int id, OrderStatus status)
        {
            var order = await _orderRepository.GetById(id, o => o.OrderItems);

            if (order == null)
                throw new Exception("Order not found");

            order.OrderStatus = status;
            order.LastUpdated = DateTime.Now;

            await _orderRepository.Update(order);
            await _orderRepository.Save();

            return MapOrderToDto(order);
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            await _orderRepository.Delete(id);
            await _orderRepository.Save();
            return true;
        }

        public async Task<bool> RestoreOrderAsync(int id)
        {
            var result = await _orderRepository.Restore(id);
            if (result)
                await _orderRepository.Save();
            return result;
        }

        public async Task<List<OrderDto>> GetDeletedOrdersAsync()
        {
            var deletedOrders = await _orderRepository.GetAllDeleted();
            return deletedOrders.Select(o => MapOrderToDto(o)).ToList();
        }

        // Helper Methods
        private decimal CalculateDiscount(decimal subtotal, DateTime orderDate)
        {
            decimal discount = 0;

            // Happy hour discount (20% off from 3-5 PM)
            if (orderDate.Hour >= 15 && orderDate.Hour < 17)
                discount += subtotal * 0.20m;

            // Bulk discount (10% off orders over $100)
            if (subtotal > 100)
                discount += subtotal * 0.10m;

            return discount;
        }

        private decimal CalculateTax(decimal subtotal, decimal discountAmount)
        {
            return (subtotal - discountAmount) * 0.085m;
        }

        private decimal CalculateTotal(decimal subtotal, decimal discountAmount, decimal taxAmount)
        {
            return subtotal - discountAmount + taxAmount;
        }

        private DateTime CalculateEstimatedDeliveryTime(List<OrderItem> orderItems)
        {
            int maxPrepTime = 30;

            foreach (var item in orderItems)
            {
                if (item.MenuItem?.PreparationTime > maxPrepTime)
                    maxPrepTime = item.MenuItem.PreparationTime;
            }

            return DateTime.Now.AddMinutes(maxPrepTime + 30);
        }

        // Manual Mapping
        private OrderDto MapOrderToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                CustomerPhone = order.CustomerPhone,
                CustomerEmail = order.CustomerEmail,
                OrderType = order.OrderType,
                OrderStatus = order.OrderStatus,
                OrderDate = order.OrderDate,
                DeliveryAddress = order.DeliveryAddress,
                Subtotal = order.Subtotal,
                TaxAmount = order.TaxAmount,
                DiscountAmount = order.DiscountAmount,
                TotalAmount = order.TotalAmount,
                EstimatedDeliveryTime = order.EstimatedDeliveryTime,
                TotalPreparationTime = order.TotalPreparationTime,
                LastUpdated = order.LastUpdated,
                userId = order.ApplicationUserId,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemsDto
                {
                    Id = oi.Id,
                    MenuItemId = oi.MenuItemId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Subtotal = oi.Subtotal,
                    OrderId = oi.OrderId,
                    MenuItem = oi.MenuItem != null ? MapMenuItemToDto(oi.MenuItem) : null
                }).ToList() ?? new List<OrderItemsDto>()
            };
        }

        private ItemsDto MapMenuItemToDto(MenuItem menuItem)
        {
            return new ItemsDto
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                CategoryId = menuItem.CategoryId,
                Quantity = menuItem.Quantity,
                ImageUrl = menuItem.ImageUrl,
                PreparationTime = menuItem.PreparationTime,
                DailyOrderCount = menuItem.DailyOrderCount,
                LastResetDate = menuItem.LastResetDate
            };
        }
    }
}