using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.Contracts;
using Models;

namespace Applications.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepo;

        public OrderService(IOrderRepository orderRepo)
        {
            this.orderRepo = orderRepo;
        }

        public async Task<List<Order>> GetAll()
        {
            return await orderRepo.GetAll();
        }
    }
}
