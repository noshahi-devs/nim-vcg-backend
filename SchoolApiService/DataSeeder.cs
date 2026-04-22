using Microsoft.AspNetCore.Identity;
using SchoolApp.Models.DataModels;
using SchoolApp.Models.DataModels.SecurityModels;
using SchoolApp.DAL.SchoolContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Seeder] Critical error during user seeding: {ex.Message}");
            }
        }

        public static async Task SeedAcademicMonths(SchoolDbContext context)
        {
            try
            {
                if (!await context.dbsAcademicMonths.AnyAsync())
                {
                    // Adding months WITHOUT specifying IDs. 
                    // SQL Identity will naturally assign 1 to 12 in this order.
                    var months = new List<AcademicMonth>
                    {
                        new AcademicMonth { MonthName = "January" },
                        new AcademicMonth { MonthName = "February" },
                        new AcademicMonth { MonthName = "March" },
                        new AcademicMonth { MonthName = "April" },
                        new AcademicMonth { MonthName = "May" },
                        new AcademicMonth { MonthName = "June" },
                        new AcademicMonth { MonthName = "July" },
                        new AcademicMonth { MonthName = "August" },
                        new AcademicMonth { MonthName = "September" },
                        new AcademicMonth { MonthName = "October" },
                        new AcademicMonth { MonthName = "November" },
                        new AcademicMonth { MonthName = "December" }
                    };

                    await context.dbsAcademicMonths.AddRangeAsync(months);
                    await context.SaveChangesAsync();
                    Console.WriteLine("[Seeder] Successfully seeded Academic Months (1-12).");
                }
                else
                {
                    Console.WriteLine("[Seeder] Academic Months already exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Seeder] Error seeding Academic Months: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[Seeder] Inner Exception: {ex.InnerException.Message}");
                }
            }
        }
    }
}
