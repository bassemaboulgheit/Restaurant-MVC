using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure
{
    public class RestaurantDb : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public RestaurantDb(DbContextOptions<RestaurantDb> options) : base(options)
        {
        }

        //public RestaurantDb() : base()
        //{
        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog= Restaurant  ;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
        //    base.OnConfiguring(optionsBuilder);
        //}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = null;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);    /////

            modelBuilder.Entity<Category>()
     .HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<MenuItem>()
                .HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<Order>()
                .HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<OrderItem>()
                .HasQueryFilter(oi => !oi.IsDeleted);

            modelBuilder.Entity<MenuItem>()
                .HasOne(m => m.Category)
                .WithMany(c => c.MenuItems)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany()
                .HasForeignKey(oi => oi.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            var dateTimeNow = new DateTime(2025, 10, 14, 12, 00, 00);


            // 🟩 Seed Categories
            modelBuilder.Entity<Category>().HasData(
                 new Category { Id = 1, Name = "Appetizers", CreatedAt = dateTimeNow },
                 new Category { Id = 2, Name = "Main Dishes", CreatedAt = dateTimeNow },
                 new Category { Id = 3, Name = "Desserts", CreatedAt = dateTimeNow },
                 new Category { Id = 4, Name = "Drinks", CreatedAt = dateTimeNow }
             );

            // 🟦 Seed MenuItems
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem { Id = 1, Name = "Spring Rolls", Price = 45m, Description = "Crispy rolls stuffed with vegetables.", Quantity = 10, CategoryId = 1, CreatedAt = dateTimeNow },
                new MenuItem { Id = 2, Name = "Grilled Chicken", Price = 120m, Description = "Juicy grilled chicken with herbs.", Quantity = 10, CategoryId = 2, CreatedAt = dateTimeNow },
                new MenuItem { Id = 3, Name = "Beef Burger", Price = 95m, Description = "Classic beef burger with cheese and fries.", Quantity = 10, CategoryId = 2, CreatedAt = dateTimeNow },
                new MenuItem { Id = 4, Name = "Chocolate Cake", Price = 60m, Description = "Rich chocolate cake with icing.", Quantity = 10, CategoryId = 3, CreatedAt = dateTimeNow },
                new MenuItem { Id = 5, Name = "Orange Juice", Price = 30m, Description = "Freshly squeezed orange juice.", Quantity = 10, CategoryId = 4, CreatedAt = dateTimeNow },
                new MenuItem { Id = 6, Name = "Caesar Salad", Price = 50m, Description = "Crisp romaine lettuce with Caesar dressing.", Quantity = 10, CategoryId = 1, CreatedAt = dateTimeNow },
                new MenuItem { Id = 7, Name = "Spaghetti Bolognese", Price = 110m, Description = "Classic Italian pasta with meat sauce.", Quantity = 10, CategoryId = 2, CreatedAt = dateTimeNow },
                new MenuItem { Id = 8, Name = "Tiramisu", Price = 70m, Description = "Coffee-flavored Italian dessert.", Quantity = 10, CategoryId = 3, CreatedAt = dateTimeNow },
                new MenuItem { Id = 9, Name = "Lemonade", Price = 25m, Description = "Refreshing lemon drink.", Quantity = 10, CategoryId = 4, CreatedAt = dateTimeNow }
           );
        }
    }
}
