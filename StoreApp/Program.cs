using StoreApp.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly); // API controller yap�land�rmas�

builder.Services.AddControllersWithViews(); //**
builder.Services.AddRazorPages(); // Razor pages yap�land�rmas�


builder.Services.ConfigureDbContext(builder.Configuration); // Extensions i�indeki ServiceExtensions metodu i�inden geliyor
builder.Services.ConfigureIdentity(); // Extensions i�indeki ServiceExtensions metodu i�inden geliyor
builder.Services.ConfigureSession();  // Extensions i�indeki ServiceExtensions metodu i�inden geliyor
builder.Services.ConfigureRepositoryRegistration(); // Extensions i�indeki ServiceExtensions metodu i�inden geliyor
builder.Services.ConfigureServiceRegistration(); // Extensions i�indeki ServiceExtensions metodu i�inden geliyor
builder.Services.ConfigureRouting(); // Extensions i�indeki ServiceExtensions metodu i�inden geliyor
builder.Services.ConfigureApplicationCookie(); // Extensions i�indeki ServiceExtensions metodu i�inden geliyor


builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();


app.UseStaticFiles(); // Root klas�r�n� kullanmak i�in
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

    endpoints.MapRazorPages(); // Razor pages yap�land�rmas�

    endpoints.MapControllers(); // API controller yap�land�rmas�
}); //***


app.ConfigureAndCheckMigration(); // Otomatik migration almak i�in yazd���m�z fonksiyon
app.ConfigureLocalization(); // Localization i�in yazd���m�z fonksiyon

await app.ConfigureDefaultAdminUser(); // Default admin user olu�turma fonksiyonu

app.Run();
