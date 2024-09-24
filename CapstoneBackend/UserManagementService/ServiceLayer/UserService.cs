using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementService.Models;
using UserManagementService.Repository;

namespace UserManagementService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _repository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _repository.GetUserByUsernameAsync(username);
        }

        public async Task RegisterUserAsync(User user)
        {
            await _repository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _repository.UpdateUserAsync(user);
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _repository.CheckEmailExistsAsync(email);
        }

        public async Task<bool> CheckUserNameExistsAsync(string userName)
        {
            return await _repository.CheckUserNameExistsAsync(userName);
        }
    }
}
