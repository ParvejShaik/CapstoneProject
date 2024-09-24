using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementService.Models;

namespace UserManagementService.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByUsernameAsync(string username);
        Task RegisterUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<bool> CheckUserNameExistsAsync(string userName);
    }
}
