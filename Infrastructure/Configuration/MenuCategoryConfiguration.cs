using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Infrastructure.Configuration
{
    public class MenuCategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(mc => mc.Id);
            builder.Property(mc => mc.Name).IsRequired().HasMaxLength(100);
            builder.Property(mc => mc.Description).HasMaxLength(500);



            // Seed data
            builder.HasData(
                new Category { Id = 1, Name = "Appetizers", Description = "Start your meal with our delicious starters"},
                new Category { Id = 2, Name = "Main Courses", Description = "Hearty and satisfying entrees" },
                new Category { Id = 3, Name = "Desserts", Description = "Sweet treats to end your meal" },
                new Category { Id = 4, Name = "Beverages", Description = "Refreshing drinks and specialty beverages" }
            );

        }
    }
}
