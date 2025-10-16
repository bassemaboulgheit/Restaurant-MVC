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
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly RestaurantDb context;

        public MenuItemRepository(RestaurantDb context)
        {
            this.context = context;
        }

        public async Task<List<MenuItem>> GetAll()
        {
            var items = await context.MenuItems.Include(c=>c.Category).ToListAsync();
            return items;
        }
        public async Task<MenuItem?> GetById(int itemId)
        {
            var item = await context.MenuItems.Include(c=>c.Category).FirstOrDefaultAsync(i=>i.Id == itemId);
            return item;
        }
        public async Task<MenuItem> GetByName(string name)
        {
            var item = await context.MenuItems.FirstOrDefaultAsync(c => c.Name.Contains(name));
            return item;
        }
        //public async Task<List<MenuItem>> GetListByName(string name)
        //{
        //    var items = await context.MenuItems.Where(i => i.Name.Contains(name)).ToListAsync();
        //    return items;
        //}
        public async Task Create(MenuItem item)
        {
            await context.MenuItems.AddAsync(item);
        }

        public async Task Update(MenuItem item)
        {
            context.MenuItems.Update(item);
        }

        public async Task Delete(int itemId)
        {
            var item = await context.MenuItems.FindAsync(itemId);
            if (item != null)
            {
                item.IsDeleted = true;
            }
        }
        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
