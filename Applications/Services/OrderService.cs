using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.Contracts;
using Applications.DTos;
using Applications.DTos.OrderDTOs;
using Applications.DTos.OrderItemsDTOs;
using Models;

namespace Applications.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(IGenericRepository<Order> _orderRepo)
        {
            this._orderRepo = _orderRepo;
        }

        public async Task<List<OrderDto>> GetAll()
        {
            var orders = await _orderRepo.GetAll(o=> o.OrderItems, o=> o.ApplicationUser);
            var ordersDto = orders.Select(i => new OrderDto
            {
                Id = i.Id,
                OrderType = i.OrderType,
                OrderStatus = i.OrderStatus,
                OrderDate = i.OrderDate,
                DeliveryAddress = i.DeliveryAddress,
                Subtotal = i.Subtotal,
                TaxAmount = i.TaxAmount,
                DiscountAmount = i.DiscountAmount,
                TotalAmount = i.TotalAmount,
                EstimatedDeliveryTime = i.EstimatedDeliveryTime,
                userId = i.ApplicationUserId,
                OrderItems = i.OrderItems.Select(oi => new OrderItemsDto
                {
                    Id = oi.Id,
                    MenuItemId = oi.MenuItemId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    OrderId = oi.OrderId
                }).ToList()
            }).ToList();

            return ordersDto;
        }
        public async Task<OrderDto?> GetById(int orderId)
        {
            var order = await _orderRepo.GetById(orderId);
            var orderDto = new OrderDto
            {
                Id = order.Id,
                OrderType = order.OrderType,
                OrderStatus = order.OrderStatus,
                OrderDate = order.OrderDate,
                DeliveryAddress = order.DeliveryAddress,
                Subtotal = order.Subtotal,
                TaxAmount = order.TaxAmount,
                DiscountAmount = order.DiscountAmount,
                TotalAmount = order.TotalAmount,
                EstimatedDeliveryTime = order.EstimatedDeliveryTime,
                userId = order.ApplicationUserId,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemsDto
                {
                    Id = oi.Id,
                    MenuItemId = oi.MenuItemId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    OrderId = oi.OrderId
                }).ToList()
            };
            return orderDto;
        }
        public async Task Create(OrderDto orderDto)
        {
            var order = new Order
            {
                OrderType = orderDto.OrderType,
                OrderStatus = orderDto.OrderStatus,
                OrderDate = orderDto.OrderDate,
                DeliveryAddress = orderDto.DeliveryAddress,
                Subtotal = orderDto.Subtotal,
                TaxAmount = orderDto.TaxAmount,
                DiscountAmount = orderDto.DiscountAmount,
                TotalAmount = orderDto.TotalAmount,
                EstimatedDeliveryTime = orderDto.EstimatedDeliveryTime,
                ApplicationUserId = orderDto.userId,
                OrderItems = orderDto.OrderItems.Select(oi => new OrderItem
                {
                    MenuItemId = oi.MenuItemId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };
            await _orderRepo.Create(order);
            await _orderRepo.Save();
        }

        public async Task CancelOrder(OrderDto orderDto)
        {
        }

        public async Task Delete(int orderId)
        {
            var order = await _orderRepo.GetById(orderId);
            if (order != null)
            {
                await _orderRepo.Delete(order.Id);
                await _orderRepo.Save();
            }
        }
    }
}
