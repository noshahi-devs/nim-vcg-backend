using Microsoft.AspNetCore.Identity;
using SchoolApp.Models.DataModels.SecurityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApiService
{
    public static class DataSeeder
    {
        public static async Task SeedAdminUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            try
            {
                // Seed Roles
                string[] roles = { "Admin", "Teacher", "Staff", "Student", "Parent", "Principal", "Accountant" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole(role));
                        if (result.Succeeded)
                        {
                            Console.WriteLine($"[Seeder] Successfully seeded role: {role}");
                        }
                        else
                        {
                            Console.WriteLine($"[Seeder] Failed to seed role {role}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }

                // Seed Admin User
                var adminEmail = "noshahi@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        Name = "Admin",
                        Status = "Active",
                        EmailConfirmed = true,
                        Role = new List<string> { "Admin" },
                        CreatedOn = DateTime.Now.ToString("yyyy-MM-dd")
                    };

                    var result = await userManager.CreateAsync(adminUser, "Noshahi.000");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRolesAsync(adminUser, new List<string> { "Admin" });
                        Console.WriteLine($"[Seeder] Successfully seeded admin user: {adminEmail}");
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        Console.WriteLine($"[Seeder] Failed to seed admin user: {errors}");
                    }
                }
                else
                {
                    Console.WriteLine($"[Seeder] Admin user {adminEmail} already exists.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Seeder] Critical error during seeding: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[Seeder] Inner Exception: {ex.InnerException.Message}");
                }
            }
        }
    }
}
