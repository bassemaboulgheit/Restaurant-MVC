using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.Contracts;
using Applications.DTos.ItemDTOs;
using Applications.DTos.OrderDTOs;
using Applications.DTos.OrderItemsDTOs;
using Models;

namespace Applications.Services
{
    public interface IOrderService
    {
        public Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto, string userId);
        public Task<OrderDto> GetOrderByIdAsync(int id);
        public Task<List<OrderDto>> GetUserOrdersAsync(string userId);
        public Task<List<OrderDto>> GetAllOrdersAsync();
        public Task<OrderDto> UpdateOrderAsync(int id, OrderDto orderDto);
        public Task<OrderDto> UpdateOrderStatusAsync(int id, OrderStatus status);
        public Task<bool> DeleteOrderAsync(int id);
        public Task<bool> Cancel(int id);
        public Task<bool> RestoreOrderAsync(int id);
        public Task<List<OrderDto>> GetDeletedOrdersAsync();
    }
}
