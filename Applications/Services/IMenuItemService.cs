using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.DTos.ItemDTOs;
using Models;

namespace Applications.Services
{
    public interface IMenuItemService
    {
        public Task<List<ItemsDto>> GetAll();
        public Task<ItemsDto?> GetById(int id);
        public Task<ItemsDto?> GetItemByName(string name);
        public Task Create(CreateItemsDto newItem);
        public Task Update(ItemsDto newItem);
        //public Task Delete(int id);
        public Task<(bool Success, string Message)> Delete(int id);
        public Task<bool> HasOrders(int itemId);
        public Task<List<ItemsDto?>> GetListItemByName(string name);
    }
}
