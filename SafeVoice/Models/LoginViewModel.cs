using System.ComponentModel.DataAnnotations;

namespace SafeVoice.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be 6-100 characters")]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}