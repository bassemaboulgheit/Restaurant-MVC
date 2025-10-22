using Microsoft.AspNetCore.Http;
using Models;

namespace Applications.Services
{
    public interface ICartService
    {
        Cart GetCart(ISession session);
        void SaveCart(ISession session, Cart cart);
        void AddItem(ISession session, CartItem item);
        void RemoveItem(ISession session, int menuItemId);
        void ClearCart(ISession session);
        decimal GetTotalPrice(ISession session);
    }
}