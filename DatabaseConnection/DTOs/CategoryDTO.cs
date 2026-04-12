using System.ComponentModel.DataAnnotations;
public class CategoryDTO
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; } = null!;


}