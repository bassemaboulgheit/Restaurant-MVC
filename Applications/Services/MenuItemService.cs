using Applications.Contracts;
using Applications.DTos;
using Models;

namespace Applications.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository itemRepo;

        public MenuItemService(IMenuItemRepository itemRepo)
        {
            this.itemRepo = itemRepo;
        }
        public async Task<List<ItemsDto>> GetAll()
        {
            var items = await itemRepo.GetAll();
            var itemDto = items.Select(item => new ItemsDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Quantity = item.Quantity,
                ImageUrl = item.ImageUrl,
                CategoryId = item.CategoryId,
                Category = new CategoryDto
                {
                    Id = item.Category.Id,
                    Name = item.Category.Name
                }

            }).ToList();
            return itemDto;
        }
        public async Task<ItemsDto?> GetById(int id)
        {
            var menuItem = await itemRepo.GetById(id);
            if (menuItem == null) return null;
            return new ItemsDto
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                ImageUrl = menuItem.ImageUrl,
                CategoryId = menuItem.CategoryId,
                Category = new CategoryDto
                {
                    Id = menuItem.Category.Id,
                    Name = menuItem.Category.Name
                },
            };
        }
        public async Task<ItemsDto> GetByName(string name)
        {
            var item = await itemRepo.GetByName(name);
            if (item == null) return null;
            var itemDto = new ItemsDto
            {
                Name = item.Name
            };

            return itemDto;
        }
        public async Task Create(ItemsDto newItem)
        {
            if (newItem == null)
            {
                return;
            }
            var menuitem = new MenuItem
            {
                Name = newItem.Name,
                Description = newItem.Description,
                Price = newItem.Price,
                Quantity = newItem.Quantity,
                ImageUrl = newItem.ImageUrl,
                CategoryId = newItem.CategoryId
            };
            await itemRepo.Create(menuitem);
            await itemRepo.Save();
        }
        public async Task Update(ItemsDto newItem)
        {
            var item = await itemRepo.GetById(newItem.Id);
            if (item == null)
            {
                return;
            }
            item.Name = newItem.Name;
            item.Description = newItem.Description;
            item.Price = newItem.Price;
            item.Quantity = newItem.Quantity;
            item.ImageUrl = newItem.ImageUrl; 
            item.CategoryId = newItem.CategoryId;

            await itemRepo.Update(item);
            await itemRepo.Save();
        }
        public async Task Delete(int id)
        {
            var item = await itemRepo.GetById(id);
            if (item == null)
            {
                return;
            }
            await itemRepo.Delete(id);
            await itemRepo.Save();
        }
    }
}
