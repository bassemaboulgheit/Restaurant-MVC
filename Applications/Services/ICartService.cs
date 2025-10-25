using Microsoft.AspNetCore.Http;
using Models;

namespace Applications.Services
{
    public interface ICartService
    {
       public Cart GetCart(ISession session);
       public void SaveCart(ISession session, Cart cart);
       public Task AddItem(ISession session, CartItem item);
       public void RemoveItem(ISession session, int menuItemId);
       public void ClearCart(ISession session);
       public decimal GetTotalPrice(ISession session);
    }
}