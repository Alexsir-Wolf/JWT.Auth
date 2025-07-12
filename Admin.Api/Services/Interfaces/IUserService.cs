using Admin.Api.DTOs;
using Admin.Api.DTOs.User;
using Admin.Api.Model;

namespace Admin.Api.Services.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(string id);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User> CreateUserAsync(CreateUserDTO createUserDto);
    Task UpdateUserAsync(string id, UpdateUserDTO updateUserDto);
    Task DeleteUserAsync(string id);
}