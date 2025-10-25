    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.DTos.CategoryDTOs;
using Applications.DTos.ItemDTOs;
using Applications.DTos.OrderDTOs;
using Applications.DTos.OrderItemsDTOs;
using Applications.DTos.UsersDTo;
using Mapster;
using Models;

namespace Applications.Mapping
{
    public static class MapsterConfig
    {
        public static void RegisterMapsterConfiguration()
        {
            // ============ MenuItem Mappings ============

            // MenuItem -> ItemsDto
            TypeAdapterConfig<MenuItem, ItemsDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl)
                .Map(dest => dest.PreparationTime, src => src.PreparationTime)
                .Map(dest => dest.DailyOrderCount, src => src.DailyOrderCount)
                .Map(dest => dest.LastResetDate, src => src.LastResetDate)
                .Map(dest => dest.Category, src => src.Category);

            // CreateItemsDto -> MenuItem
            TypeAdapterConfig<CreateItemsDto, MenuItem>.NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl)
                .Map(dest => dest.PreparationTime, src => src.PreparationTime)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.IsDeleted)
                .Ignore(dest => dest.DailyOrderCount)
                .Ignore(dest => dest.LastResetDate)
                .Ignore(dest => dest.Category)
                .Ignore(dest => dest.OrderItems);

            // ItemsDto -> MenuItem (for Update)
            TypeAdapterConfig<ItemsDto, MenuItem>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl)
                .Map(dest => dest.PreparationTime, src => src.PreparationTime)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Ignore(dest => dest.IsDeleted)
                .Ignore(dest => dest.Category)
                .Ignore(dest => dest.OrderItems);

            // ============ Order Mappings ============

            // Order -> OrderDto
            TypeAdapterConfig<Order, OrderDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.CustomerName, src => src.CustomerName)
                .Map(dest => dest.CustomerPhone, src => src.CustomerPhone)
                .Map(dest => dest.CustomerEmail, src => src.CustomerEmail)
                .Map(dest => dest.OrderType, src => src.OrderType)
                .Map(dest => dest.OrderStatus, src => src.OrderStatus)
                .Map(dest => dest.OrderDate, src => src.OrderDate)
                .Map(dest => dest.DeliveryAddress, src => src.DeliveryAddress)
                .Map(dest => dest.Subtotal, src => src.Subtotal)
                .Map(dest => dest.TaxAmount, src => src.TaxAmount)
                .Map(dest => dest.DiscountAmount, src => src.DiscountAmount)
                .Map(dest => dest.TotalAmount, src => src.TotalAmount)
                .Map(dest => dest.EstimatedDeliveryTime, src => src.EstimatedDeliveryTime)
                .Map(dest => dest.TotalPreparationTime, src => src.TotalPreparationTime)
                .Map(dest => dest.LastUpdated, src => src.LastUpdated)
                .Map(dest => dest.userId, src => src.ApplicationUserId)
                .Map(dest => dest.OrderItems, src => src.OrderItems);

            // CreateOrderDto -> Order
            TypeAdapterConfig<CreateOrderDto, Order>.NewConfig()
                .Map(dest => dest.CustomerName, src => src.CustomerName)
                .Map(dest => dest.CustomerPhone, src => src.CustomerPhone)
                .Map(dest => dest.CustomerEmail, src => src.CustomerEmail)
                .Map(dest => dest.OrderType, src => src.OrderType)
                .Map(dest => dest.DeliveryAddress, src => src.DeliveryAddress)
                .Map(dest => dest.ApplicationUserId, src => src.userId)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.IsDeleted)
                .Ignore(dest => dest.OrderStatus)
                .Ignore(dest => dest.OrderDate)
                .Ignore(dest => dest.Subtotal)
                .Ignore(dest => dest.TaxAmount)
                .Ignore(dest => dest.DiscountAmount)
                .Ignore(dest => dest.TotalAmount)
                .Ignore(dest => dest.EstimatedDeliveryTime)
                .Ignore(dest => dest.TotalPreparationTime)
                .Ignore(dest => dest.LastUpdated)
                .Ignore(dest => dest.ApplicationUser)
                .Ignore(dest => dest.OrderItems);

            // OrderDto -> Order (for Update)
            TypeAdapterConfig<OrderDto, Order>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.CustomerName, src => src.CustomerName)
                .Map(dest => dest.CustomerPhone, src => src.CustomerPhone)
                .Map(dest => dest.CustomerEmail, src => src.CustomerEmail)
                .Map(dest => dest.DeliveryAddress, src => src.DeliveryAddress)
                .Map(dest => dest.ApplicationUserId, src => src.userId)
                .Ignore(dest => dest.IsDeleted)
                .Ignore(dest => dest.ApplicationUser);

            // ============ OrderItem Mappings ============

            // OrderItem -> OrderItemsDto
            TypeAdapterConfig<OrderItem, OrderItemsDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.MenuItemId, src => src.MenuItemId)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.UnitPrice, src => src.UnitPrice)
                .Map(dest => dest.Subtotal, src => src.Subtotal)
                .Map(dest => dest.OrderId, src => src.OrderId)
                .Map(dest => dest.MenuItem, src => src.MenuItem);

            // CreateOrderItemsDto -> OrderItem
            TypeAdapterConfig<CreateOrderItemsDto, OrderItem>.NewConfig()
                .Map(dest => dest.MenuItemId, src => src.MenuItemId)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.UnitPrice, src => src.UnitPrice)
                .Map(dest => dest.Subtotal, src => src.Subtotal)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.IsDeleted)
                .Ignore(dest => dest.Order)
                .Ignore(dest => dest.OrderId)
                .Ignore(dest => dest.MenuItem);

            // OrderItemsDto -> OrderItem (for Update)
            TypeAdapterConfig<OrderItemsDto, OrderItem>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.MenuItemId, src => src.MenuItemId)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.UnitPrice, src => src.UnitPrice)
                .Map(dest => dest.Subtotal, src => src.Subtotal)
                .Map(dest => dest.OrderId, src => src.OrderId)
                .Ignore(dest => dest.IsDeleted)
                .Ignore(dest => dest.Order)
                .Ignore(dest => dest.MenuItem);

            // ============ Category Mappings ============

            // Category -> CategoryDto
            TypeAdapterConfig<Category, CategoryDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name);

            // CreateCategoryDto -> Category
            TypeAdapterConfig<CreateCategoryDto, Category>.NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.IsDeleted)
                .Ignore(dest => dest.MenuItems);

            // CategoryDto -> Category (for Update)
            TypeAdapterConfig<CategoryDto, Category>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Ignore(dest => dest.IsDeleted)
                .Ignore(dest => dest.MenuItems);

            // ============ User Mappings ============

            // RegisterDto -> ApplicationUser
            TypeAdapterConfig<RegisterDto, ApplicationUser>.NewConfig()
                .Map(dest => dest.UserName, src => src.userName)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Address, src => src.Address)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.PasswordHash)
                .Ignore(dest => dest.Orders)
                .Ignore(dest => dest.NormalizedUserName)
                .Ignore(dest => dest.NormalizedEmail)
                .Ignore(dest => dest.EmailConfirmed)
                .Ignore(dest => dest.SecurityStamp)
                .Ignore(dest => dest.ConcurrencyStamp)
                .Ignore(dest => dest.PhoneNumber)
                .Ignore(dest => dest.PhoneNumberConfirmed)
                .Ignore(dest => dest.TwoFactorEnabled)
                .Ignore(dest => dest.LockoutEnd)
                .Ignore(dest => dest.LockoutEnabled)
                .Ignore(dest => dest.AccessFailedCount);

            // ApplicationUser -> LoginDto (Optional - if needed)
            TypeAdapterConfig<ApplicationUser, LoginDto>.NewConfig()
                .Map(dest => dest.userName, src => src.UserName)
                .Ignore(dest => dest.Password)
                .Ignore(dest => dest.RememberMe);
        }
    }
}
