using System.ComponentModel.DataAnnotations;

public class AgeAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime date)
        {
            var age = DateTime.Now.Year - date.Year;
            Console.WriteLine(age);
            if (age > 20) return true;
        }

        return false;

    }
}