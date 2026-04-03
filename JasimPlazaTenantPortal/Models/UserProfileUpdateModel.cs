using System.ComponentModel.DataAnnotations;

namespace Jasim_Plaza_Tenant_Portal.Models;

public class UserProfileUpdateModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}
