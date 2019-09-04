using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MSS.PlasticSurgery.Data
{
    public static class Seed
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            //adding customs roles
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] roleNames = { "Admin", "Manager", "Member" };

            foreach (var roleName in roleNames)
            {
                // creating the roles and seeding them to the database
                var roleExist = await roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var configurationSection = configuration.GetSection("AdminSettings");

            // creating a super user, who controls web app
            var adminUser = new IdentityUser()
            {
                UserName = configurationSection["Email"],
                Email = configurationSection["Email"]
            };

            string adminPassword = configurationSection["Password"];
            var user = await userManager.FindByEmailAsync(configurationSection["Email"]);

            if (user == null)
            {
                var createPowerUser = await userManager.CreateAsync(adminUser, adminPassword);

                if (createPowerUser.Succeeded)
                {
                    // assigning the new user to the "Admin" role
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
