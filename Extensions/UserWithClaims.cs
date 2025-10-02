using Automotive_Project.Models;
using System.Security.Claims;

namespace Automotive_Project.Extensions
{
    public class UserWithClaims
    {
        public int Id { get; set; }
        public UserAccount User { get; set; }
        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        public UserWithClaims(UserAccount user, ClaimsPrincipal claimsPrincipal)
        {
            User = user;
            ClaimsPrincipal = claimsPrincipal;
        }
    }

}
