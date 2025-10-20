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
        private readonly RestaurantDb _context;
        DbSet<T> _dbSet;
        public GenericRepository(RestaurantDb _context)
        {
            this._context = _context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IQueryable<T>> GetAll( Expression<Func<T, object>>[]? includes = null)
        //public async Task<IQueryable<T>> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<T> query = _dbSet;
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

            return  query;
        }

        public async Task<T?> GetById(int id , params Expression<Func<T, object>>[] includes)   
        {
            IQueryable<T> query = _dbSet;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }


        public async Task<bool> GetByName(string name , params Expression<Func<T, object>>[] includes)
        {
            return await _dbSet.AnyAsync(e => EF.Property<string>(e, "Name").Contains(name));
        }


        public async Task<List<T>> GetListByName(string name, params Expression<Func<T, object>>[] includes)
        {
            return await _dbSet.Where(e => EF.Property<string>(e, "Name").Contains(name)).ToListAsync();
        }


        public async Task<T?> GetName(string name, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query
                    .AsQueryable()
                    .FirstOrDefaultAsync(e =>
                        (EF.Property<string>(e, "Name") ?? string.Empty)
                            .ToLower()
                            .Contains(name.ToLower())
                    );
        }

        public async Task Create(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _dbSet.Update(entity);
            }
        }

        public async Task<bool> Restor(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null && entity.IsDeleted)
            {
                entity.IsDeleted = false;
                _dbSet.Update(entity);
                return true;
            }
            return false;
        }

        public async Task<List<T>> GetAllDeleted()
        {
             return await _dbSet.IgnoreQueryFilters().ToListAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
