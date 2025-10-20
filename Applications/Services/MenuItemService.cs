using Applications.Contracts;
using Applications.DTos;
using Models;

namespace Applications.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IGenericRepository<MenuItem> _itemRepo;

        public MenuItemService(IGenericRepository<MenuItem> _itemRepo)
        {
            this._itemRepo = _itemRepo;
        }
        public async Task<List<ItemsDto>> GetAll()
        {
            var items = await _itemRepo.GetAll(c => c.Category);
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
            var menuItem = await _itemRepo.GetById(id,c=>c.Category);
            if (menuItem == null) return null;
            return new ItemsDto
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Quantity = menuItem.Quantity,
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

        public async Task<ItemsDto?> GetItemByName(string name)
        {
            var menuItem = await _itemRepo.GetName(name, i => i.Category);

            if (menuItem == null)
                return null;

            var itemDto = new ItemsDto()
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                Quantity = menuItem.Quantity,
                ImageUrl = menuItem.ImageUrl,
                CategoryId = menuItem.CategoryId,
                Category = menuItem.Category == null
                    ? null
                    : new CategoryDto
                    {
                        Id = menuItem.Category.Id,
                        Name = menuItem.Category.Name
                    }
            };
            return itemDto;
        }


        public async Task<bool> GetByName(string name)
        {
            var item = await _itemRepo.GetByName(name);
            if (item == null)
            {
                return false;
            }
            return  item;
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
            await _itemRepo.Create(menuitem);
            await _itemRepo.Save();
        }
        public async Task Update(ItemsDto newItem)
        {
            var item = await _itemRepo.GetById(newItem.Id);
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

            await _itemRepo.Update(item);
            await _itemRepo.Save();
        }
        public async Task Delete(int id)
        {
            var item = await _itemRepo.GetById(id);
            if (item == null)
            {
                return;
            }
            await _itemRepo.Delete(id);
            await _itemRepo.Save();
        }
    }
}
