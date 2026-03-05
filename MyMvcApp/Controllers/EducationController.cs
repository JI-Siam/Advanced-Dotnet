using Microsoft.AspNetCore.Mvc;

public class EducationController : Controller
{

    public IActionResult EducationDetails()
    {
        EducationItem ed1 = new EducationItem()
        {
            Cgpa = 3.9,
            Degree = "BSc. CSE"
        };


        EducationItem ed2 = new EducationItem()
        {
            Cgpa = 3.5,
            Degree = "BSc. CSE"
        };

        EducationItem[] items = [ed1, ed2];

        return View(items);
    }

}