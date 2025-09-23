using Automotive_Project.Models;

namespace Automotive_Project.Services
{
    public interface IRoleRepository
    {
        Task<AppRole> GetByNameAsync(string roleName);
        Task CreateAsync(AppRole role);
    }
}
