using Microsoft.AspNetCore.Mvc;

public class StudentController : Controller
{


    // Student/Index/{semester}
    public IActionResult Index(int id)

    {

        var data = new List<Student>();
        Random rand = new Random();

        for (int i = 1; i <= 10; i++)
        {
            data.Add(new Student()
            {
                Name = "Student " + i,
                Id = "Id " + i,
                Cgpa = (float)rand.NextInt64(2, 4) + rand.NextDouble(),
                Age = (int)rand.NextInt64(18, 25),
                Semester = (int)rand.NextInt64(4, 12),
                Fee = (int)rand.NextInt64(20000, 100000)
            });
        }

        var filteredData = new List<Student>();

        if (id != 0)
        {
            foreach (var student in data)
            {
                if (student.Semester == id) filteredData.Add(student);
            }
            return View(filteredData);
        }

        return View(data);
    }

    public IActionResult Profile(string id)
    {
        Random rand = new Random();
        Student student = new Student()
        {
            Name = "Student " + id,
            Id = id,
            Cgpa = (float)rand.NextInt64(2, 4) + rand.NextDouble(),
            Age = (int)rand.NextInt64(18, 25),
            Semester = (int)rand.NextInt64(4, 12),
            Fee = (int)rand.NextInt64(20000, 100000)
        };

        return View(student);
    }
}

// http://localhost:5297/Student/Index

