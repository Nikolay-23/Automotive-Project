using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data;
using System.Security.Claims;

namespace Automotive_Project.Models
{
    public class CustomSignInManager<TUser> where TUser : class
    {
        private readonly CustomUserManager<TUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomSignInManager(CustomUserManager<TUser> userManager, IHttpContextAccessor accessor)
        {
            _userManager = userManager;
            _httpContextAccessor = accessor;
        }

        public async Task<bool> PasswordSignInAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                return false;

            var claims = new List<Claim>
            {
             
             new Claim(ClaimTypes.Name, user.Email),
             new Claim("UserName", user.UserName ?? ""),
             new Claim("FirstName", user.FirstName ?? ""),
             new Claim("LastName", user.LastName ?? ""),
             new Claim("FullName", $"{user.FirstName} {user.LastName}")

            };

            


            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return true;
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public bool IsSignedIn(ClaimsPrincipal user)
        {
            return user?.Identity != null && user.Identity.IsAuthenticated;
        }
    }
}
