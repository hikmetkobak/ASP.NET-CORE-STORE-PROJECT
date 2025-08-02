using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;
using Services;
using Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); //**
builder.Services.AddRazorPages(); // Razor pages yap�land�rmas�

//Dependency Injecion'da kullan�l�yor
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

builder.Services.AddSingleton<Cart>(); // Tek nesne i�in servis kayd�

builder.Services.AddAutoMapper(typeof(Program));



var app = builder.Build();


app.UseStaticFiles(); // Root klas�r�n� kullanmak i�in
app.UseHttpsRedirection(); //**
app.UseRouting(); //** 

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name:"Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

    endpoints.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages(); // Razor pages yap�land�rmas�
}); //***

    

app.Run();
