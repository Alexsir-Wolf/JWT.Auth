using Admin.Api.DTOs;
using Admin.Api.Model;
using Microsoft.AspNetCore.Mvc;
using Admin.Api.Services.Interfaces;

namespace Admin.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
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
        var user = await _userService.CreateUserAsync(createUserDto);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("update-user/{id}", Name = "Update User")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserDTO updateUserDto)
    {
        await _userService.UpdateUserAsync(id, updateUserDto);
        return NoContent();
    }

    [HttpDelete("delete/{id}", Name = "Delete User")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}