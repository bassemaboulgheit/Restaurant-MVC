using System.Security.Claims;
using System.Threading.Tasks;
using Applications.Contracts;
using Applications.DTos.CategoryDTOs;
using Applications.DTos.ItemDTOs;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Applications.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryService(
            IGenericRepository<Category> categoryRepo,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private bool IsCurrentUserAdmin()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.IsInRole("Admin") ?? false;
        }

        public async Task<List<CategoryDto>> GetAll()
        {
            bool isAdmin = IsCurrentUserAdmin();
            var categories = await _categoryRepo.GetAll(i => i.MenuItems);

            if (isAdmin)
            {
                return categories.Select(category => new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Items = category.MenuItems?
                        .Where(item => !item.IsDeleted)
                        .Select(item => item.Adapt<ItemsDto>())
                        .ToList() ?? new List<ItemsDto>()
                }).ToList();
            }
            else
            {
                return categories
                    .Where(c => c.MenuItems != null && c.MenuItems.Any(m => m.Quantity > 0 && !m.IsDeleted))
                    .Select(category => new CategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = category.Description,
                        Items = category.MenuItems?
                            .Where(item => item.Quantity > 0 && !item.IsDeleted)
                            .Select(item => item.Adapt<ItemsDto>())
                            .ToList() ?? new List<ItemsDto>()
                    }).ToList();
            }
        }

        public async Task<CategoryDto?> GetById(int id)
        {
            bool isAdmin = IsCurrentUserAdmin();
            var menuCategory = await _categoryRepo.GetById(id, i => i.MenuItems);

            if (menuCategory == null)
                return null;

            var categoryDto = new CategoryDto
            {
                Id = menuCategory.Id,
                Name = menuCategory.Name,
                Description = menuCategory.Description
            };

            if (isAdmin)
            {
                categoryDto.Items = menuCategory.MenuItems?
                    .Where(item => !item.IsDeleted)
                    .Select(item => item.Adapt<ItemsDto>())
                    .ToList() ?? new List<ItemsDto>();
            }
            else
            {
                categoryDto.Items = menuCategory.MenuItems?
                    .Where(item => item.Quantity > 0 && !item.IsDeleted)
                    .Select(item => item.Adapt<ItemsDto>())
                    .ToList() ?? new List<ItemsDto>();

                if (categoryDto.Items.Count == 0)
                    return null;
            }

            return categoryDto;
        }

        public async Task<CategoryDto?> GetCategoryByName(string name)
        {
            bool isAdmin = IsCurrentUserAdmin();
            var menuCategory = await _categoryRepo.GetName(name, c => c.MenuItems);

            if (menuCategory == null)
                return null;

            var categoryDto = new CategoryDto
            {
                Id = menuCategory.Id,
                Name = menuCategory.Name,
                Description = menuCategory.Description
            };

            if (isAdmin)
            {
                categoryDto.Items = menuCategory.MenuItems?
                    .Where(item => !item.IsDeleted)
                    .Select(item => item.Adapt<ItemsDto>())
                    .ToList() ?? new List<ItemsDto>();
            }
            else
            {
                categoryDto.Items = menuCategory.MenuItems?
                    .Where(item => item.Quantity > 0 && !item.IsDeleted)
                    .Select(item => item.Adapt<ItemsDto>())
                    .ToList() ?? new List<ItemsDto>();

                if (categoryDto.Items.Count == 0)
                    return null;
            }

            return categoryDto;
        }

        public async Task Create(CreateCategoryDto newCategory)
        {
            if (newCategory == null)
                return;

            var category = newCategory.Adapt<Category>();

            await _categoryRepo.Create(category);
            await _categoryRepo.Save();
        }

        public async Task Update(CategoryDto categoryDto)
        {
            var category = await _categoryRepo.GetById(categoryDto.Id);
            if (category == null)
                return;

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;

            await _categoryRepo.Update(category);
            await _categoryRepo.Save();
        }

        public async Task Delete(int id)
        {
            var category = await _categoryRepo.GetById(id, c => c.MenuItems);
            if (category == null)
                return;

            await _categoryRepo.Delete(id);

            if (category.MenuItems != null)
            {
                foreach (var item in category.MenuItems)
                {
                    item.IsDeleted = true;
                }
            }

            await _categoryRepo.Save();
        }

        public async Task<bool> GetByName(string name)
        {
            var category = await _categoryRepo.GetByName(name);
            return category;
        }
    }
}