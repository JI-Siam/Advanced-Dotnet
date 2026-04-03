using System.ComponentModel.DataAnnotations;

namespace Jasim_Plaza_Tenant_Portal.Models;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
