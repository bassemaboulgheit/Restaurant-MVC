using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Applications.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAll(params Expression<Func<T, object>>[] includes);
        Task<T?> GetById(int id, params Expression<Func<T, object>>[] includes);
        Task<bool> GetByName(string name);
        Task<List<T>> GetListByName(string name);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(int id);
        Task Save();
    }
}
