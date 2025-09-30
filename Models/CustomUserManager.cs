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
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
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

        public async Task<UserWithClaims?> GetUserAsync(ClaimsPrincipal principal)
        {
            if (principal == null || !principal.Identity?.IsAuthenticated == true)
                return null;

            var email = principal.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var user = await _db.UserAccounts
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (user == null)
                return null;

            var claims = new List<Claim>
            {
             new Claim(ClaimTypes.Name, user.Email ?? string.Empty),
             new Claim("UserName", user.UserName ?? string.Empty),
             new Claim("FirstName", user.FirstName ?? string.Empty),
             new Claim("LastName", user.LastName ?? string.Empty),
             new Claim("FullName", $"{user.FirstName} {user.LastName}".Trim())
            };

            claims.AddRange(user.Roles
                .Where(r => !string.IsNullOrWhiteSpace(r.Name))
                .Select(r => new Claim(ClaimTypes.Role, r.Name)));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            return new UserWithClaims(user, claimsPrincipal);
        }

        public async Task<List<UserAccount>> GetUsersInRoleAsync(string roleName)
        {
            return await _db.UserAccounts
                .Include(u => u.Roles)
                .Where(u => u.Roles.Any(r => r.Name == roleName))
                .ToListAsync();
        }


        public async Task<bool> SetAdminAsync(UserAccount user)
        {
            if (user == null)
                return false;

            var adminRole = await _db.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole != null)
            {
                var usersWithAdmin = await _db.UserAccounts
                    .Include(u => u.Roles)
                    .Where(u => u.Roles.Any(r => r.Name == "Admin"))
                    .ToListAsync();

                foreach (var u in usersWithAdmin)
                {
                    u.Roles.Remove(adminRole);
                }
            }
            else
            {
                adminRole = new AppRole { Name = "Admin" };
                _db.Roles.Add(adminRole);
            }

            if (!user.Roles.Contains(adminRole))
                user.Roles.Add(adminRole);

            await _db.SaveChangesAsync();
            return true;
        }
    }

}
