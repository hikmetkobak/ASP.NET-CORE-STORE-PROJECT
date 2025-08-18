using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace StoreApp.Infrastructure.Extensions
{
    public static class ApplicationExtension
    {
        #region Otomatik Migration
        public static void ConfigureAndCheckMigration(this IApplicationBuilder app)    // otomatik update database yapan extension
        {
            RepositoryContext context = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<RepositoryContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
        #endregion

        #region Localization
        public static void ConfigureLocalization(this IApplicationBuilder app) // 
        {
            app.UseRequestLocalization(options =>
            {
                options.AddSupportedCultures("tr-TR")
                .AddSupportedUICultures("tr-Tr")
                .SetDefaultCulture("tr-TR");
            });
        }
        #endregion

        #region Admin User Configuration
        public static async Task ConfigureDefaultAdminUser(this IApplicationBuilder app)
        {
            const string adminUser = "Admin";
            const string adminPassword = "Admin@123456"; // Şifre politikasına uygun hale getirildi

            using var scope = app.ApplicationServices.CreateScope();
            
            // User Manager
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //Role Manager
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var user = await userManager.FindByNameAsync(adminUser);
            if (user is not null) return;

            user = new IdentityUser
            {
                Email = "hkobak@coskunoz.com.tr",
                PhoneNumber = "05321234567",
                UserName = adminUser
            };

            var result = await userManager.CreateAsync(user, adminPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Admin user could not be created. Errors: {errors}");
            }

            var roles = roleManager.Roles.Select(r => r.Name).ToList(); // Roles
            if (roles.Count == 0)
            {
                throw new Exception("No roles found in the system to assign to admin.");
            }

            var roleResult = await userManager.AddToRolesAsync(user, roles);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"Role assignment failed. Errors: {errors}");
            }
        }
        #endregion
    }

}

