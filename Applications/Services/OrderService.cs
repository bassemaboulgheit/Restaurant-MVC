using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Applications.Contracts;
using Applications.DTos.ItemDTOs;
using Applications.DTos.OrderDTOs;
using Applications.DTos.OrderItemsDTOs;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Applications.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderItem> _orderItemRepository;
        private readonly IGenericRepository<MenuItem> _menuItemRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IGenericRepository<Order> orderRepository,
            IGenericRepository<OrderItem> orderItemRepository,
            IGenericRepository<MenuItem> menuItemRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _menuItemRepository = menuItemRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto, string userId)
        {
            decimal subtotal = 0;
            var orderItems = new List<OrderItem>();

            foreach (var itemDto in createOrderDto.OrderItems)
            {
                var menuItem = await _menuItemRepository.GetById(itemDto.MenuItemId);
                if (menuItem == null)
                    throw new Exception($"Menu item with ID {itemDto.MenuItemId} not found");

                if (menuItem.Quantity < itemDto.Quantity)
                    throw new Exception($"Not enough stock for item: {menuItem.Name}");

                var orderItem = itemDto.Adapt<OrderItem>();
                orderItem.MenuItem = menuItem;
                orderItem.Subtotal = itemDto.Quantity * itemDto.UnitPrice;

                subtotal += orderItem.Subtotal;
                orderItems.Add(orderItem);
            }

            decimal discountAmount = CalculateDiscount(subtotal, createOrderDto.OrderDate);
            decimal taxAmount = CalculateTax(subtotal, discountAmount);
            decimal totalAmount = CalculateTotal(subtotal, discountAmount, taxAmount);

            var order = createOrderDto.Adapt<Order>();
            order.OrderStatus = OrderStatus.Pending;
            order.OrderDate = DateTime.Now;
            order.ApplicationUserId = userId;
            order.Subtotal = subtotal;
            order.DiscountAmount = discountAmount;
            order.TaxAmount = taxAmount;
            order.TotalAmount = totalAmount;
            order.OrderItems = orderItems;
            order.LastUpdated = DateTime.Now;

            if (createOrderDto.OrderType == OrderType.Delivery)
            {
                order.EstimatedDeliveryTime = CalculateEstimatedDeliveryTime(orderItems);
            }

            await _orderRepository.Create(order);
            await _orderRepository.Save();

            foreach (var orderItem in orderItems)
            {
                var menuItem = await _menuItemRepository.GetById(orderItem.MenuItemId);
                if (menuItem != null)
                {
                    if (menuItem.LastResetDate.Date < DateTime.Today)
                    {
                        menuItem.DailyOrderCount = 0;
                        menuItem.LastResetDate = DateTime.Today;
                    }

                    menuItem.DailyOrderCount += orderItem.Quantity;

                    if (menuItem.Quantity >= orderItem.Quantity)
                        menuItem.Quantity -= orderItem.Quantity;
                    else
                        throw new Exception($"Not enough quantity for item {menuItem.Name}");

                    if (menuItem.DailyOrderCount >= 50)
                    {
                        menuItem.Quantity = 0;
                    }

                    await _menuItemRepository.Update(menuItem);
                    await _menuItemRepository.Save();
                }
            }

            return order.Adapt<OrderDto>();
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetById(id,
                o => o.OrderItems,
                o => o.ApplicationUser);

            if (order == null)
                throw new Exception("Order not found");

            if (order.OrderItems != null)
            {
                foreach (var item in order.OrderItems)
                {
                    if (item.MenuItem == null && item.MenuItemId > 0)
                    {
                        item.MenuItem = await _menuItemRepository.GetById(item.MenuItemId);
                    }
                }
            }

            return order.Adapt<OrderDto>();
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAll(
                o => o.OrderItems,
                o => o.ApplicationUser);

            foreach (var order in orders)
            {
                if (order.OrderItems != null)
                {
                    foreach (var item in order.OrderItems)
                    {
                        if (item.MenuItem == null && item.MenuItemId > 0)
                        {
                            item.MenuItem = await _menuItemRepository.GetById(item.MenuItemId);
                        }
                    }
                }
            }

            return orders.Adapt<List<OrderDto>>();
        }

        public async Task<List<OrderDto>> GetUserOrdersAsync(string userId)
        {
            var orders = await _orderRepository.GetAll(
                o => o.OrderItems,
                o => o.ApplicationUser);

            var userOrders = orders.Where(o => o.ApplicationUserId == userId).ToList();

            foreach (var order in userOrders)
            {
                if (order.OrderItems != null)
                {
                    foreach (var item in order.OrderItems)
                    {
                        if (item.MenuItem == null && item.MenuItemId > 0)
                        {
                            item.MenuItem = await _menuItemRepository.GetById(item.MenuItemId);
                        }
                    }
                }
            }

            return userOrders.Adapt<List<OrderDto>>();
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

            return order.Adapt<OrderDto>();
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

            return order.Adapt<OrderDto>();
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetById(id);
            if (order.OrderStatus == OrderStatus.Ready || order.OrderStatus == OrderStatus.Delivered)
                return false;

            await _orderRepository.Delete(id);
            await _orderRepository.Save();
            return true;
        }

        public async Task<bool> Cancel(int id)
        {
            var order = await _orderRepository.GetById(id);
            if (order == null)
                return false;
            if (order.OrderStatus == OrderStatus.Delivered || order.OrderStatus == OrderStatus.Ready)
                return false;

            order.OrderStatus = OrderStatus.Cancelled;

            foreach (var item in order.OrderItems)
            {
                var menuItem = await _menuItemRepository.GetById(item.MenuItemId);
                if (menuItem != null)
                {
                    menuItem.Quantity += item.Quantity;
                    await _menuItemRepository.Update(menuItem);
                }
            }

            await _menuItemRepository.Save();
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
            return deletedOrders.Adapt<List<OrderDto>>();
        }

        // Helper Methods
        private decimal CalculateDiscount(decimal subtotal, DateTime orderDate)
        {
            decimal discount = 0;

            if (orderDate.Hour >= 15 && orderDate.Hour < 17)
                discount += subtotal * 0.20m;

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
    }
}