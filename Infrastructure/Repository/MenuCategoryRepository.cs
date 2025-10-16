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
    public class MenuCategoryRepository : IMenuCategoryRepository
    {
        private readonly RestaurantDb context;

        public MenuCategoryRepository(RestaurantDb context)
        {
            this.context = context;
        }

        public async Task<List<Category>> GetAll()
        {
            var categories = await context.Categories.Include(i => i.MenuItems).ToListAsync();
            return categories;
        }
        public async Task<Category?> GetById(int categoryId)
        {
            var category = await context.Categories.Include(i => i.MenuItems).FirstOrDefaultAsync(i => i.Id == categoryId);
            return category;
        }
        public async Task Create(Category category)
        {
            await context.Categories.AddAsync(category);
        }

        public async Task Update(Category category)
        {
             context.Categories.Update(category);
        }

        public async Task Delete(int categoryId)
        {
            var category = await context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                category.IsDeleted = true;
                foreach(var item in category.MenuItems)
                {
                    item.IsDeleted = true;
                }
            }
        }
        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task<Category> GetByName(string name)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Name.Contains(name));
            return category;
        }
        public async Task<bool> GetByName1(string name)
        {
            var category = await context.Categories.AnyAsync(c => c.Name.Contains(name));
            return category;
        }

        //public async Task<List<Category>> GetListByName(string name)
        //{
        //    var category = await context.Categories.Where(c => c.Name.Contains(name)).ToListAsync();
        //    return category;
        //}

    }
}
