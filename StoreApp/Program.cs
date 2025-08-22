using StoreApp.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly); // API controller yapýlandýrmasý

builder.Services.AddControllersWithViews(); //**
builder.Services.AddRazorPages(); // Razor pages yapýlandýrmasý


builder.Services.ConfigureDbContext(builder.Configuration); // Extensions içindeki ServiceExtensions metodu içinden geliyor
builder.Services.ConfigureIdentity(); // Extensions içindeki ServiceExtensions metodu içinden geliyor
builder.Services.ConfigureSession();  // Extensions içindeki ServiceExtensions metodu içinden geliyor
builder.Services.ConfigureRepositoryRegistration(); // Extensions içindeki ServiceExtensions metodu içinden geliyor
builder.Services.ConfigureServiceRegistration(); // Extensions içindeki ServiceExtensions metodu içinden geliyor
builder.Services.ConfigureRouting(); // Extensions içindeki ServiceExtensions metodu içinden geliyor
builder.Services.ConfigureApplicationCookie(); // Extensions içindeki ServiceExtensions metodu içinden geliyor


builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();


app.UseStaticFiles(); // Root klasörünü kullanmak için
app.UseSession(); // Session kullanma metodu
app.UseHttpsRedirection(); //**
app.UseRouting(); //** 

app.UseAuthentication(); // 
app.UseAuthorization(); // 

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name:"Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

    endpoints.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages(); // Razor pages yapýlandýrmasý

    endpoints.MapControllers(); // API controller yapýlandýrmasý
}); //***


app.ConfigureAndCheckMigration(); // Otomatik migration almak için yazdýðýmýz fonksiyon
app.ConfigureLocalization(); // Localization için yazdýðýmýz fonksiyon

await app.ConfigureDefaultAdminUser(); // Default admin user oluþturma fonksiyonu

app.Run();
