using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.DTos.CategoryDTOs;
using Models;

namespace Applications.Services
{
    public interface ICategoryService
    {
        public Task<List<CategoryDto>> GetAll();
        public Task<CategoryDto?> GetById(int id);
        //public Task<CategoryDto?> GetByName(string name);
        public Task<CategoryDto> GetCategoryByName(string name);
        public Task<bool> GetByName(string name);
        public Task Create(CreateCategoryDto menuCategory);
        public Task Update(CategoryDto menuCategory);
        public Task Delete(int id);
    }
}
