using Admin.Api.Model;

namespace Admin.Api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(string id);
    Task<User?> GetUserByUsernameAsync(string username);
    Task CreateUserAsync(User user);
    Task UpdateUserAsync(string id, User user);
    Task DeleteUserAsync(string id);
}