using Microsoft.AspNetCore.Mvc;

public class CourseController : Controller
{

    public IActionResult List(int id)
    {

        var rand = new Random();
        var data = new List<Course>();

        for (int i = 1; i <= 10; i++)
        {
            data.Add(new Course
            {
                Id = i,
                Title = "Title" + i,
                Credit = (int)rand.NextInt64(1, 3),
                Semester = (int)rand.NextInt64(1, 12)
            });
        }

        var filteredData = new List<Course>();

        if (id != 0)
        {
            foreach (var item in data)
            {
                if (item.Semester == id)
                {
                    filteredData.Add(item);
                }
            }


            return View(filteredData);

        }


        return View(data);
    }

}