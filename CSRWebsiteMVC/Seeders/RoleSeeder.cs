using System;
using Microsoft.AspNetCore.Identity;

namespace CSRWebsiteMVC.Seeders;

public class RoleSeeder
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var role in roleNames)
            {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
            }
        }

}
