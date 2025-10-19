using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Applications.Contracts;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly RestaurantDb context;
        DbSet<T> dbSet;
        public GenericRepository(RestaurantDb context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public async Task<List<T>> GetAll( Expression<Func<T, object>>[]? includes = null)
        //public async Task<IQueryable<T>> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            //if (filter != null)
            //{ query = query.Where(filter); }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            //if (!tracked)
            //{
            //    query = query.AsNoTracking();
            //}
            //return  query;

            return  await  query.ToListAsync();
        }

        public async Task<T?> GetById(int id , params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<bool> GetByName(string name)
        {
            //return await dbSet.FirstOrDefaultAsync(e => EF.Property<string>(e, "Name").Contains(name));
            return await dbSet.AnyAsync(e => EF.Property<string>(e, "Name").Contains(name));
        }
        public async Task<List<T>> GetListByName(string name)
        {
            return await dbSet.Where(e => EF.Property<string>(e, "Name").Contains(name)).ToListAsync();
        }

        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task Update(T entity)
        {
            dbSet.Update(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                dbSet.Update(entity);
            }
        }

        public async Task<bool> Restor(int id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity != null && entity.IsDeleted)
            {
                entity.IsDeleted = false;
                dbSet.Update(entity);
                return true;
            }
            return false;
        }

        public async Task<List<T>> GetAllDeleted()
        {
             return await dbSet.IgnoreQueryFilters().ToListAsync();
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
