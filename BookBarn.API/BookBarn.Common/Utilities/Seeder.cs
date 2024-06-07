using BookBarn.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BookBarn.Common.Utilities
{
    public class Seeder
    {
        public static async Task SeedRolesAndAdmins(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Seed roles
            await SeedRole(roleManager, "Admin");
            await SeedRole(roleManager, "User");

            // Seed admin user
            await SeedAdmin(userManager, "Admin", "bookbarnadmin@gmail.com", "Password@123", "BookbarnAdmin");
        }

        private static async Task SeedRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        private static async Task SeedAdmin(UserManager<AppUser> userManager, string roleName, string email, string password, string firstName)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}
