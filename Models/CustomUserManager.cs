using Automotive_Project.Data;
using Automotive_Project.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Automotive_Project.Models
{
    public class CustomUserManager<TUser> where TUser : class
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

        public async Task<OperationResult<UserAccount>> CreateAsync(UserAccount user, string password)
        {
            try
            {
                user.Password = PasswordHasher.HashPassword(password);
                user.CreatedAt = DateTime.UtcNow;

                _db.UserAccounts.Add(user);
                await _db.SaveChangesAsync();

                return OperationResult<UserAccount>.Success(user);
            }
            catch (Exception ex)
            {
                return OperationResult<UserAccount>.Failed(ex.Message);
            }
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

        public async Task<ClaimsPrincipal?> GetUserAsync(string email)
        {
            var user = await _db.UserAccounts
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim("UserName", user.UserName),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName),
            new Claim("FullName", $"{user.FirstName} {user.LastName}")
        };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }

        public async Task<List<UserAccount>> GetUsersInRoleAsync(string roleName)
        {
            return await _db.UserAccounts
                .Include(u => u.Roles)
                .Where(u => u.Roles.Any(r => r.Name == roleName))
                .ToListAsync();
        }
    }

}
