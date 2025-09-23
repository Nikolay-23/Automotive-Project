using Automotive_Project.Models;

namespace Automotive_Project.Services
{
    public interface IUserRepository
    {
        Task<UserAccount> GetByUsernameAsync(string username);
        Task<UserAccount> GetByEmailAsync(string email);
        Task CreateAsync(UserAccount user);
        Task UpdateAsync(UserAccount user);
    }
}
