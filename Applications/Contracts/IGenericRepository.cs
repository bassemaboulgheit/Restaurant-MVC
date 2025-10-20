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
        Task<IQueryable<T>> GetAll(params Expression<Func<T, object>>[] includes);

        //public Task<IQueryable<T>> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

        Task<T?> GetById(int id, params Expression<Func<T, object>>[] includes);
        Task<bool> GetByName(string name, params Expression<Func<T, object>>[] includes);
        Task<T?> GetName(string name, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetListByName(string name, params Expression<Func<T, object>>[] includes);

        Task Create(T entity);
        Task Update(T entity);
        Task Delete(int id);
        Task Save();
    }
}
