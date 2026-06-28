using ecommerceAPI.src.EcommerceAPI.Domain.Constants;
using ecommerceAPI.src.EcommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ecommerceAPI.src.EcommerceAPI.Persistence.Seeds
{
    public static class AdminSeed
    {
        public static async Task SeedAdminAsync(IServiceProvider services, IConfiguration configuration)
        {
            var enabled = configuration.GetValue<bool>("SeedAdmin:Enabled");
            if (!enabled)
            {
                return;
            }

            var username = configuration["SeedAdmin:Username"];
            var email = configuration["SeedAdmin:Email"];
            var password = configuration["SeedAdmin:Password"];

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("SeedAdmin configuration is incomplete.");
            }

            var userManager = services.GetRequiredService<UserManager<User>>();

            var existingUser = await userManager.FindByNameAsync(username)
                ?? await userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                if (existingUser.Role != UserRoles.Admin || !existingUser.IsActive)
                {
                    existingUser.Role = UserRoles.Admin;
                    existingUser.IsActive = true;
                    existingUser.UpdatedAt = DateTime.UtcNow;

                    var updateResult = await userManager.UpdateAsync(existingUser);
                    if (!updateResult.Succeeded)
                    {
                        throw new InvalidOperationException(
                            string.Join(", ", updateResult.Errors.Select(error => error.Description)));
                    }
                }

                return;
            }

            var admin = new User
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "Base",
                Role = UserRoles.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(admin, password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(error => error.Description)));
            }
        }
    }
}
