using Microsoft.AspNetCore.Identity;

namespace TechDirect.Data.Seed
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            const string adminRoleName = "Admin";

            // 1. Ensure Admin role exists
            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRoleName));
            }

            // 2. Ensure admin user exists
            var adminEmail = "admin@techdirect.local";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Site",
                    LastName = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var createResult = await userManager.CreateAsync(adminUser, "Admin123!");

                if (!createResult.Succeeded)
                {
                    throw new Exception(
                        "Failed to create admin user: " +
                        string.Join(", ", createResult.Errors.Select(e => e.Description))
                    );
                }
            }

            // 3. Assign role if not assigned
            if (!await userManager.IsInRoleAsync(adminUser, adminRoleName))
            {
                await userManager.AddToRoleAsync(adminUser, adminRoleName);
            }
        }
    }
}
