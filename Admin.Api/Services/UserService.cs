using Admin.Api.DTOs;
using Admin.Api.Model;
using Admin.Api.Services.Interfaces;
using Admin.Api.Repositories.Interfaces;

namespace Admin.Api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
 
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("ID não pode ser nulo ou vazio.", nameof(id));
        }

        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username não pode ser nulo ou vazio.", nameof(username));
        }

        return await _userRepository.GetUserByUsernameAsync(username);
    }

    public async Task<User> CreateUserAsync(CreateUserDTO createUserDto)
    {
        if (createUserDto == null)
        {
            throw new ArgumentNullException(nameof(createUserDto));
        }

        // Verificar se o username já existe
        var existingUser = await _userRepository.GetUserByUsernameAsync(createUserDto.UserName);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username já existe.");
        }

        var user = new User
        {
            UserName = createUserDto.UserName,
            Password = HashPassword(createUserDto.Password), // Hash da senha
            Role = createUserDto.Role
        };

        await _userRepository.CreateUserAsync(user);
        return user;
    }

    public async Task UpdateUserAsync(string id, UpdateUserDTO updateUserDto)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("ID não pode ser nulo ou vazio.", nameof(id));
        }

        if (updateUserDto == null)
        {
            throw new ArgumentNullException(nameof(updateUserDto));
        }

        var existingUser = await _userRepository.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"Usuário com ID {id} não encontrado.");
        }

        // Verificar se o novo username já existe (se foi alterado)
        if (existingUser.UserName != updateUserDto.UserName)
        {
            var userWithSameUsername = await _userRepository.GetUserByUsernameAsync(updateUserDto.UserName);
            if (userWithSameUsername != null)
            {
                throw new InvalidOperationException("Username já existe.");
            }
        }

        existingUser.UserName = updateUserDto.UserName;
        existingUser.Role = updateUserDto.Role;
            
        // Só atualiza a senha se foi fornecida
        if (!string.IsNullOrEmpty(updateUserDto.Password))
        {
            existingUser.Password = HashPassword(updateUserDto.Password);
        }

        await _userRepository.UpdateUserAsync(id, existingUser);
    }

    public async Task DeleteUserAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("ID não pode ser nulo ou vazio.", nameof(id));
        }

        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            throw new InvalidOperationException($"Usuário com ID {id} não encontrado.");
        }

        await _userRepository.DeleteUserAsync(id);
    }

    private string HashPassword(string password)
    {
        // Em produção, use BCrypt ou outra biblioteca de hash segura
        // Para este exemplo, retorno a senha sem hash
        // Exemplo com BCrypt: return BCrypt.Net.BCrypt.HashPassword(password);
        return password;
    }
}