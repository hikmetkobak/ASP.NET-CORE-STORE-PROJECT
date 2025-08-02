using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;
using Services;
using Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); //**
builder.Services.AddRazorPages(); // Razor pages yapýlandýrmasý

//Dependency Injecion'da kullanýlýyor
builder.Services.AddDbContext<RepositoryContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("sqlconnection"), b => b.MigrationsAssembly("StoreApp"));
});

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>(); //**
builder.Services.AddScoped<IProductRepository, ProductRepository>(); //**
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); //**

builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IProductService,ProductManager>();
builder.Services.AddScoped<ICategoryService,CategoryManager>();

builder.Services.AddSingleton<Cart>(); // Tek nesne için servis kaydý

builder.Services.AddAutoMapper(typeof(Program));



var app = builder.Build();


app.UseStaticFiles(); // Root klasörünü kullanmak için
app.UseHttpsRedirection(); //**
app.UseRouting(); //** 

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name:"Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

    endpoints.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages(); // Razor pages yapýlandýrmasý
}); //***

    

app.Run();
