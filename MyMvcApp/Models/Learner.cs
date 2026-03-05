using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

public class Learner
{

    [Required]
    [RegularExpression(@"^[a-zA-Z .-]+$")]
    public string Name
    {
        set; get;
    }

    [Required]
    [RegularExpression(@"^\d{2}-\d{5}-[123]$", ErrorMessage = "Id must in this format xx-xxxxx-x")]
    public string Id
    {
        set; get;
    }

    [Required]
    [RegularExpression(@"^\d{2}-\d{5}-[123]@student.aiub.edu$", ErrorMessage = "Email must in this format xx-xxxxx-x@student.aiub.edu")]
    public string Email
    {
        set; get;
    }

    [Age(ErrorMessage = "Age Must be greater than 20")]
    public string Dob
    {
        set; get;
    }

}