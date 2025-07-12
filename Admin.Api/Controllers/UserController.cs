using Admin.Api.DTOs;
using Admin.Api.DTOs.User;
using Admin.Api.Infrastructure.Interfaces;
using Admin.Api.Model;
using Microsoft.AspNetCore.Mvc;
using Admin.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Admin.Api.Controllers;

[ApiController]
[Route("api/user")]
[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IPasswordService _passwordService;
    
    public UserController(IUserService userService, IPasswordService passwordService)
    {
        _userService = userService;
        _passwordService = passwordService;
    }

    [HttpGet("get-users", Name = "Get Users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("get-by-id/{id}", Name = "Get by Id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        
        if (user == null)
        {
            return NotFound($"Usuário com ID {id} não encontrado.");
        }

        return Ok(user);
    }

    [HttpGet("get-by-username/{username}",  Name = "Get by Username")]
    public async Task<ActionResult<User>> GetUserByUsername(string username)
    {
        var user = await _userService.GetUserByUsernameAsync(username);
        
        if (user == null)
        {
            return NotFound($"Usuário de username '{username}' não encontrado.");
        }

        return Ok(user);
    }

    [HttpPost("add-user", Name = "Add User")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<User>> CreateUser(CreateUserDTO createUserDto)
    {
        var newUser = new CreateUserDTO
        {
            UserName = createUserDto.UserName,
            Password = _passwordService.Encrypt(createUserDto.Password),
            Role = createUserDto.Role
        };
        
        var user = await _userService.CreateUserAsync(newUser);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("update-user/{id}", Name = "Update User")]
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserDTO updateUserDto)
    {
        var updateUser = new UpdateUserDTO
        {
            UserName = updateUserDto.UserName,
            Password = _passwordService.Encrypt(updateUserDto.Password),
            Role = updateUserDto.Role
        };
        
        await _userService.UpdateUserAsync(id, updateUser);
        return Ok(updateUserDto);
    }

    [HttpDelete("delete/{id}", Name = "Delete User")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}