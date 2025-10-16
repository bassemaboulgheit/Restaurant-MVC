using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Applications.Contracts
{
    public interface IMenuCategoryRepository
    {
        public  Task<List<Category>> GetAll();
        public Task<Category?> GetById(int categoryId);
        public Task<Category?> GetByName(string name);
        public Task<bool> GetByName1(string name);
        public Task Create(Category category);
        public Task Update(Category category);
        public Task Delete(int categoryId);
        public Task Save();
        //public Task<List<Category>> GetListByName(string name);
    }
}
