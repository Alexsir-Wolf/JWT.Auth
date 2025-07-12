using Admin.Api.DTOs.Login;
using Microsoft.AspNetCore.Mvc;
using Admin.Api.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Admin.Api.Controllers;


public class AuthController : Controller
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login", Name = "login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginDTO>> Login(LoginDTO loginDto)
    {
        var token = _tokenService.GenerateToken(loginDto);

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized();
        }
        
        return Ok(new
        {
            token = token,
            message = "Login successful"
        });
    }
}