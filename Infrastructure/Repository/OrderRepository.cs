using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RestaurantDb context;

        public OrderRepository(RestaurantDb context)
        {
            this.context = context;
        }

        public async Task<List<Order>> GetAll()
        {
            return await context.Orders.ToListAsync();
        }

        public async Task<Order?> GetById(int orderId)
        {
            return await context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        }
        public async Task Create(Order order)
        {
            await context.Orders.AddAsync(order);
        }
        public async Task Update(Order order)
        {
             context.Orders.Update(order);
        }
        public async Task Delete(int orderId)
        {
            var order = await context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.IsDeleted = true;
            }
        }
        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
