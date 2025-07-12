using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Admin.Api.DTOs.Login;
using Admin.Api.Infrastructure.Interfaces;
using Admin.Api.Model;
using Admin.Api.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Admin.Api.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    
    public TokenService(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }
    
    public string GenerateToken(LoginDTO loginDto)
    {
        var userDataBase = _userRepository.GetUserByUsernameAsync(loginDto.UserName).Result;
        if (loginDto.UserName != userDataBase.UserName || loginDto.Password != userDataBase.Password)
            return string.Empty;
        
        var secretKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? string.Empty));
        var issuer = _configuration["Jwt:Issuer"] ?? string.Empty;
        var audience = _configuration["Jwt:Audience"] ?? string.Empty;
        
        var signingCredentials = new SigningCredentials(
            secretKey, 
            SecurityAlgorithms.HmacSha256);
        
        var tokenOptions = new JwtSecurityToken(
           issuer: issuer,
           audience: audience,
           claims:
           [
               new  Claim(ClaimTypes.Name, userDataBase.UserName),
               new  Claim(ClaimTypes.Role, userDataBase.Role)
           ],
           expires: DateTime.Now.AddHours(1),
           signingCredentials: signingCredentials);
        
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}