using Applications.Contracts;
using Microsoft.AspNetCore.Http;
using Models;
using Newtonsoft.Json;

namespace Applications.Services
{
    public class CartService : ICartService
    {
        private const string CartSessionKey = "UserCart";
        private readonly IGenericRepository<MenuItem> _menuItemRepository;

        public CartService(IGenericRepository<MenuItem> menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public Cart GetCart(ISession session)
        {
            var cartJson = session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(cartJson)
                ? new Cart()
                : JsonConvert.DeserializeObject<Cart>(cartJson) ?? new Cart();
        }

        public void SaveCart(ISession session, Cart cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            session.SetString(CartSessionKey, cartJson);
        }

        public async Task AddItem(ISession session, CartItem item)
        {
            var menuItem = await _menuItemRepository.GetById(item.MenuItemId);

            if (menuItem == null || menuItem.Quantity <= 0)
            {
                throw new Exception($"Item '{item.Name}' is no longer available");
            }

            var cart = GetCart(session);
            var existingItem = cart.CartItems.FirstOrDefault(i => i.MenuItemId == item.MenuItemId);

            if (existingItem != null)
            {
                if (existingItem.Quantity + item.Quantity > menuItem.Quantity)
                {
                    throw new Exception($"Only {menuItem.Quantity} units available");
                }
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                if (item.Quantity > menuItem.Quantity)
                {
                    throw new Exception($"Only {menuItem.Quantity} units available");
                }
                cart.CartItems.Add(item);
            }

            SaveCart(session, cart);
        }

        public void RemoveItem(ISession session, int menuItemId)
        {
            var cart = GetCart(session);
            cart.CartItems.RemoveAll(i => i.MenuItemId == menuItemId);
            SaveCart(session, cart);
        }

        public void ClearCart(ISession session)
        {
            var cart = new Cart();
            SaveCart(session, cart);
        }

        public decimal GetTotalPrice(ISession session)
        {
            var cart = GetCart(session);
            return cart.CartItems.Sum(i => i.Total);
        }
    }
}