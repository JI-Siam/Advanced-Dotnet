using System.ComponentModel.DataAnnotations;

public class RegistrationModel
{

    [Required(ErrorMessage = "Custom Message")]
    [StringLength(100, MinimumLength = 3)]

    public string Name
    {
        set; get;
    }

    [Required]
    [StringLength(8)]
    public string Pass
    {
        set; get;
    }
}