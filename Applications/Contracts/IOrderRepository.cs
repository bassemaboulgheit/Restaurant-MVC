using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Applications.Contracts
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAll();
        Task<Order?> GetById(int orderId);
        Task Create(Order order);
        Task Update(Order order);
        Task Delete(int orderId);
        Task Save();
    }
}
