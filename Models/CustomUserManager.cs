using Automotive_Project.Data;
using Automotive_Project.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Automotive_Project.Models
{
    public class CustomUserManager
    {
        private readonly ApplicationDbContext _db;

        public CustomUserManager(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<UserAccount?> FindByEmailAsync(string email)
        {
            return await _db.UserAccounts
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == email.ToLower());
        }

        public async Task<UserAccount> CreateAsync(UserAccount user, string password)
        {
            user.Password = PasswordHasher.HashPassword(password);
            user.CreatedAt = DateTime.UtcNow;

            _db.UserAccounts.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<bool> CheckPasswordAsync(UserAccount user, string password)
        {
            return user.Password == PasswordHasher.HashPassword(password);
        }

        public async Task AddToRoleAsync(UserAccount user, string roleName)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                role = new AppRole { Name = roleName };
                _db.Roles.Add(role);
            }

            user.Roles.Add(role);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserAccount user)
        {
            _db.UserAccounts.Update(user);
            await _db.SaveChangesAsync();
        }
    }

}
