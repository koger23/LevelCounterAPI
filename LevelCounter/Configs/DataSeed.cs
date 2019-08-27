using LevelCounter.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LevelCounter.Configs
{
    public class DataSeed
    {
        public static void SeedDefaultDatas(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            SeedRoles(roleManager);
            SeedAdminUser(userManager);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var roleNames = new List<string>() { "Admin", "User" };
            foreach (var name in roleNames)
            {
                if (!roleManager.RoleExistsAsync(name).Result)
                {
                    var roleResult = roleManager.CreateAsync(new IdentityRole() { Name = name }).Result;
                }
            }
        }

        private static void SeedAdminUser(UserManager<ApplicationUser> userManager)
        {
            var password = "Passw0rd";
            var users = new Dictionary<string, string>
            {
                ["admin"] = "Admin",
            };
            foreach (var userEntry in users)
            {
                if (userManager.FindByEmailAsync(userEntry.Key).Result == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = userEntry.Key,
                        Email = "admin@kogero.com",
                        Gender = ApplicationUser.Genders.MALE,
                        Statistics = new Statistics(),
                        FullName = "Admin",
                        IsPublic = false
                    };
                    var result = userManager.CreateAsync(user, password).Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, userEntry.Value).Wait();
                    }
                }
            }
        }
    }
}
