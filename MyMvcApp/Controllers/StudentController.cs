using Microsoft.AspNetCore.Mvc;
public class StudentController : Controller
{

    public IActionResult Index()
    {
        var data = new List<Student>();
        Random rand = new Random();
        for (int i = 1; i <= 10; i++)
        {
            data.Add(new Student()
            {
                Name = "Student " + i,
                Id = i,
                Cgpa = (float)(rand.NextInt64(2, 4) + rand.NextDouble()),
                Age = (int)rand.NextInt64(20, 25),
                Department = "CSE"

            });
        }
        return View(data);
    }

    public IActionResult Profile(int id)
    {
        Random rand = new Random();
        Student data = new Student()
        {
            Name = "Student " + id,
            Id = id,
            Cgpa = (float)(rand.NextInt64(2, 4) + rand.NextDouble()),
            Age = (int)rand.NextInt64(18, 25),
            Department = "CSE"

        };
        return View(data);
    }

    public IActionResult Details(int id)
    {

        Student student = new Student()
        {
            Name = "Student " + id,
            Id = id,
            Cgpa = 3.75f
        };

        return View(student);

    }

}