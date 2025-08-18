using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;
using Services;
using Entities.Models;
using StoreApp.Models;
using Microsoft.AspNetCore.Identity;

namespace StoreApp.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        #region Database Configuration
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("sqlconnection"), b => b.MigrationsAssembly("StoreApp"));
                options.EnableSensitiveDataLogging(true); // Hata ayıklama için
            });
        }
        #endregion

        #region Identity Configuration Repository içinde de ekliyoruz
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<RepositoryContext>(); // RepositoryContext ile ilişkilendiriyoruz
        }
        #endregion

        #region Session Configuration 
        public static void ConfigureSession(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache(); // Middleware Session
            services.AddSession(options =>
            {
                options.Cookie.Name = "StoreApp.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(10); // Session bilgilerinin tutulma süresi
            }); // Session ifadeleri için

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // Middleware , Httcontext ifadelerine erişmek için
            services.AddScoped<Cart>(c => SessionCart.GetCart(c)); // Tek nesne için servis kaydı
        }
        #endregion

        #region Repository ve Service Registration
        public static void ConfigureRepositoryRegistration(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>(); //**
            services.AddScoped<IProductRepository, ProductRepository>(); //**
            services.AddScoped<ICategoryRepository, CategoryRepository>(); //**
            services.AddScoped<IOrderRepository, OrderRepository>(); //**
        }

        public static void ConfigureServiceRegistration(this IServiceCollection services) 
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IOrderService, OrderManager>();
        }
        #endregion

        #region Routing lower case
        public static void ConfigureRouting(this IServiceCollection services)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = false;
            });
        }
        #endregion

    }
}
