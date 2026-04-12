
public class ProductDTO
{

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; } = null!;

    [Required]
    public double Price { get; set; }

    [Required]
    public int CategoryId { get; set; }
}