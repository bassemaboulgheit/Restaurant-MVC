using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Applications.Contracts
{
    public interface IMenuItemRepository
    {
        public  Task<List<MenuItem>> GetAll();
        public  Task<MenuItem?> GetById(int itemId);
        public  Task<MenuItem> GetByName(string name);
        //public  Task<List<MenuItem>> GetListByName(string name);
        public  Task Create(MenuItem item);
        public  Task Update(MenuItem item);
        public  Task Delete(int itemId);
        public  Task Save();
    }
}
