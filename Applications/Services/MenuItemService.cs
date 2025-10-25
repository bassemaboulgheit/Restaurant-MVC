using Applications.Contracts;
using Applications.DTos.ItemDTOs;
using Mapster;
using MapsterMapper;
using Models;

namespace Applications.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IGenericRepository<MenuItem> _itemRepo;
        private readonly IGenericRepository<OrderItem> _orderItemRepo;
        private readonly IMapper _mapper;

        public MenuItemService(
            IGenericRepository<MenuItem> itemRepo,
            IGenericRepository<OrderItem> orderItemRepo,
            IMapper mapper)
        {
            _itemRepo = itemRepo;
            _orderItemRepo = orderItemRepo;
            _mapper = mapper;
        }

        public async Task<List<ItemsDto>> GetAll()
        {
            var items = await _itemRepo.GetAll(c => c.Category);
            return items.Adapt<List<ItemsDto>>();
        }

        public async Task<ItemsDto?> GetById(int id)
        {
            var menuItem = await _itemRepo.GetById(id, c => c.Category);
            if (menuItem == null)
                return null;

            return menuItem.Adapt<ItemsDto>();
        }

        public async Task<ItemsDto?> GetItemByName(string name)
        {
            var menuItem = await _itemRepo.GetName(name, i => i.Category);
            if (menuItem == null)
                return null;

            return menuItem.Adapt<ItemsDto>();
        }

        public async Task<bool> GetByName(string name)
        {
            var item = await _itemRepo.GetByName(name);
            return item != null;
        }

        public async Task Create(CreateItemsDto newItem)
        {
            if (newItem == null)
                return;

            var menuItem = newItem.Adapt<MenuItem>();

            await _itemRepo.Create(menuItem);
            await _itemRepo.Save();
        }

        public async Task Update(ItemsDto newItem)
        {
            var item = await _itemRepo.GetById(newItem.Id);
            if (item == null)
                return;

            newItem.Adapt(item);

            await _itemRepo.Update(item);
            await _itemRepo.Save();
        }

        public async Task<(bool Success, string Message)> Delete(int id)
        {
            var item = await _itemRepo.GetById(id);
            if (item == null)
                return (false, "Item not found");

            bool hasOrders = await HasOrders(id);

            if (hasOrders)
            {
                return (false, "Cannot delete this item because it exists in one or more orders");
            }

            await _itemRepo.Delete(id);
            await _itemRepo.Save();

            return (true, "Item deleted successfully");
        }

        public async Task<List<ItemsDto?>> GetListItemByName(string name)
        {
            var menuItems = await _itemRepo.GetListByName(name, i => i.Category);
            if (menuItems == null)
                return null;

            return menuItems.Adapt<List<ItemsDto>>();
        }

        public async Task<bool> HasOrders(int itemId)
        {
            var orderItems = await _orderItemRepo.GetAll();
            return orderItems.Any(oi => oi.MenuItemId == itemId && !oi.IsDeleted);
        }
    }
}
