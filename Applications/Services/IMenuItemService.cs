using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.DTos;
using Models;

namespace Applications.Services
{
    public interface IMenuItemService
    {
        public Task<List<ItemsDto>> GetAll();
        public Task<ItemsDto?> GetById(int id);
        //public Task<ItemsDto> GetByName(string name);
        public Task<ItemsDto?> GetItemByName(string name);
        public Task Create(ItemsDto newItem);
        public Task Update(ItemsDto newItem);
        public Task Delete(int id);
    }
}
