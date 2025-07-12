namespace Admin.Api.DTOs.Login;

public record LoginDTO
{
    public string UserName { get; set; }
    public string  Password { get; set; }
}