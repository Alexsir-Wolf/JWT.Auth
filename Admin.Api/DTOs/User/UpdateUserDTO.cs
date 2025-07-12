using System.ComponentModel.DataAnnotations;

namespace Admin.Api.DTOs.User;

public class UpdateUserDTO
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string UserName { get; set; } = string.Empty;

    [StringLength(100, MinimumLength = 6)]
    public string? Password { get; set; }

    [Required]
    public string Role { get; set; } = string.Empty;
}