using Automotive_Project.Models;
using System;

namespace Automotive_Project.Data
{
    public class DatabaseInitializer
    {
        public static async Task SeedDataAsync(
            CustomUserManager<UserAccount>? customUserManager,
            CustomRoleManager<AppRole>? customRoleManager)
        {
            if (customUserManager == null || customRoleManager == null)
            {
                Console.WriteLine("customUserManager or customRoleManager is null => exit");
                return;
            }

            // Ensure roles exist
            if (!await customRoleManager.RoleExistsAsync("Admin"))
            {
                await customRoleManager.CreateRoleAsync("Admin");
            }

            if (!await customRoleManager.RoleExistsAsync("User"))
            {
                await customRoleManager.CreateRoleAsync("User");
            }

            // Check if we already have at least one admin user
            var adminUsers = await customUserManager.GetUsersInRoleAsync("Admin");
            if (adminUsers.Any())
            {
                Console.WriteLine("Admin user already exists => exit");
                return;
            }

            // Create default admin user
            var user = new UserAccount
            {
                UserName = "admin@admin.com",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@admin.com",
                CreatedAt = DateTime.UtcNow
            };

            string initialPassword = "admin123";

            var result = await customUserManager.CreateAsync(user, initialPassword);

            if (result.Succeeded)
            {
                var createdUser = result.Value!;

                // 🔥 Assign the Admin role immediately
                await customUserManager.AddToRoleAsync(createdUser, "Admin");

                Console.WriteLine("Admin user created successfully with Admin role!");
            }
            else
            {
                Console.WriteLine("Failed to create admin user:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(" - " + error);
                }
            }
        }
    }
}
