using System.Security.Claims;
using Applications.Contracts;
using Applications.Mapping;
using Applications.Services;
using Infrastructure;
using Infrastructure.Repository;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;

namespace Restaurant
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(options=>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
           
            builder.Services.AddDbContext<RestaurantDb>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<RestaurantDb>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            MapsterConfig.RegisterMapsterConfiguration();

            // Register Mapster Services
            var mapsterConfig = TypeAdapterConfig.GlobalSettings;
            builder.Services.AddSingleton(mapsterConfig);
            builder.Services.AddScoped<IMapper, Mapper>();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICategoryService,CategoryService>();
            builder.Services.AddScoped<IMenuCategoryRepository,CategoryRepository>();
            builder.Services.AddScoped<IMenuItemService, MenuItemService>();
            //builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ICartService, CartService>();

            builder.Services.AddHostedService<OrderAutoTransitionService>();
            builder.Services.AddHostedService<DailyAvailabilityService>();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMiddleware<Web_App_MVC.MiddleWare.DateMiddleWare>();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                //pattern: "{controller=Home}/{action=Index}/{id?}");

                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            //pattern: "{area=Customer}/{controller=MenuItem}/{action=getAll}/{id?}");

            app.Run();
        }
    }
}
