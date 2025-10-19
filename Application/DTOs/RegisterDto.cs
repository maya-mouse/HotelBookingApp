using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
  public class RegisterDto
  {

    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;


    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = null!;
    
    public string? RoleName { get; set; }
    }
}