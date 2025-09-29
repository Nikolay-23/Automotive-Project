using Automotive_Project.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Automotive_Project.Models
{
    public class CustomRoleManager<TUser> where TUser : class
    {
        private readonly ApplicationDbContext _db;

        public CustomRoleManager(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<AppRole> CreateRoleAsync(string roleName)
        {
            var role = new AppRole { Name = roleName };
            _db.Roles.Add(role);
            await _db.SaveChangesAsync();
            return role;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _db.Roles.AnyAsync(r => r.Name == roleName);
        }

        public async Task<AppRole?> FindByNameAsync(string roleName)
        {
            return await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task AddUserToRoleAsync(UserAccount user, string roleName)
        {
            var role = await FindByNameAsync(roleName);
            if (role == null)
            {
                role = await CreateRoleAsync(roleName);
            }

            if (!user.Roles.Any(r => r.Name == role.Name))
            {
                user.Roles.Add(role);
                _db.UserAccounts.Update(user);
                await _db.SaveChangesAsync();
            }
        }

        public List<string> GetUserRoles(UserAccount user)
        {
            return user.Roles.Select(r => r.Name).ToList();
        }
    }
}
