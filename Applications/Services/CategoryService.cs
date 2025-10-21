using System.Threading.Tasks;
using Applications.Contracts;
using Applications.DTos.CategoryDTOs;
using Applications.DTos.ItemDTOs;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Applications.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepo;

        //private readonly IMenuCategoryRepository categoryRepo;

        public CategoryService(IGenericRepository<Category> _categoryRepo)
        {
            this._categoryRepo = _categoryRepo;
        }

        public async Task<List<CategoryDto>> GetAll()
        {
            var query = await _categoryRepo.GetAll(i=>i.MenuItems);
            var categories = await query.ToListAsync();
            var categoryDtos = categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Items = category.MenuItems?.Select(item => new ItemsDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    ImageUrl = item.ImageUrl,
                    CategoryId = item.CategoryId
                }).ToList() ?? new List<ItemsDto>()
            }).ToList();
            return categoryDtos;
        }
        public async Task<CategoryDto?> GetById(int id)
        {
            var menuCategory = await _categoryRepo.GetById(id, i => i.MenuItems);
            if (menuCategory == null)
            {
                return null;
            }
            var categoryDto = new CategoryDto()
            {
                Id = menuCategory.Id,
                Name = menuCategory.Name,
                Description = menuCategory.Description,
                Items = menuCategory.MenuItems?.Select(item => new ItemsDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ImageUrl = item.ImageUrl,
                    CategoryId = item.CategoryId
                }).ToList() ?? new List<ItemsDto>()
            };
            return categoryDto;
        }
        public async Task<CategoryDto?> GetCategoryByName(string name)
        {
            var menuCategory = await _categoryRepo.GetName(name, c => c.MenuItems);
            if (menuCategory == null)
                return null;
            var categoryDto = new CategoryDto()
            {
                Id = menuCategory.Id,
                Name = menuCategory.Name,
                Description = menuCategory.Description,
                Items = menuCategory.MenuItems?.Select(item => new ItemsDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    ImageUrl = item.ImageUrl,
                    CategoryId = item.CategoryId
                }).ToList() ?? new List<ItemsDto>()
            };
            return categoryDto;
        }

        public async Task Create(CreateCategoryDto newCategory)
        {
            if (newCategory == null)
            {
                return;
            }
            var category = new Category()
            {
                Name = newCategory.Name,
                Description = newCategory.Description,
            };
            await _categoryRepo.Create(category);
            await _categoryRepo.Save();
        }
        public async Task Update(CategoryDto newCategory)
        {
            var category = await _categoryRepo.GetById(newCategory.Id);
            if (category == null)
            {
                return;
            }
            category.Name = newCategory.Name;
            category.Description = newCategory.Description;
            await _categoryRepo.Update(category);
            await _categoryRepo.Save();
        }
        public async Task Delete(int id)
        {
            var category = await _categoryRepo.GetById(id);
            if (category == null)
            {
                return;
            }
            await _categoryRepo.Delete(id);
            await _categoryRepo.Save();
        }

        public async Task<bool> GetByName(string name)
        {
            var category = await _categoryRepo.GetByName(name);
            if (category)
            {
                return category;
            }
            return false;
        }
    }
}
