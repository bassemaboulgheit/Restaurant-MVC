using System.Threading.Tasks;
using Applications.Contracts;
using Applications.DTos;
using Models;

namespace Applications.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> categoryRepo;

        //private readonly IMenuCategoryRepository categoryRepo;

        public CategoryService(IGenericRepository<Category> categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }

        public async Task<List<CategoryDto>> GetAll()
        {
            var categories = await categoryRepo.GetAll();
            //var categories = await categoryRepo.Get();
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
            var menuCategory = await categoryRepo.GetById(id, i => i.MenuItems);
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
                    ImageUrl = item.ImageUrl,
                    CategoryId = item.CategoryId
                }).ToList() ?? new List<ItemsDto>()
            };
            return categoryDto;
        }
        //public async Task<CategoryDto> GetByName(string name)
        //{
        //    var menuCategory = await categoryRepo.GetByName(name);
        //    if (menuCategory == null) {
        //        return null;
        //    }
        //    var categoryDto = new CategoryDto()
        //        {
        //        Name = menuCategory.Name
        //    };
        //    return categoryDto;
        //}
        public async Task Create(CategoryDto newCategory)
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
            await categoryRepo.Create(category);
            await categoryRepo.Save();
        }
        public async Task Update(CategoryDto newCategory)
        {
            var category = await categoryRepo.GetById(newCategory.Id);
            if (category == null)
            {
                return;
            }
            category.Name = newCategory.Name;
            category.Description = newCategory.Description;
            await categoryRepo.Update(category);
            await categoryRepo.Save();
        }
        public async Task Delete(int id)
        {
            var category = await categoryRepo.GetById(id);
            if (category == null)
            {
                return;
            }
            await categoryRepo.Delete(id);
            await categoryRepo.Save();
        }

        public async Task<bool> GetByName(string name)
        {
            var category = await categoryRepo.GetByName(name);
            if (category == null)
            {
                return false;
            }
            return  category;
        }
    }
}
