using Admin.Api.Model;

namespace Admin.Api.Infrastructure.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}