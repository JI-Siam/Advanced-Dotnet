using Microsoft.AspNetCore.Mvc;

public class EmployeeController : Controller
{

    public IActionResult List()
    {
        var data = new List<Employee>();
        string[] designations = ["Manager", "CEO", "SWE", "CDE"];
        var rand = new Random();

        for (int i = 1; i <= 10; i++)
        {
            data.Add(new Employee
            {
                Name = "Employee" + i,
                Designation = designations[(int)rand.NextInt64(0, 3)],
                Salary = rand.NextInt64(5000, 100000)
            });
        }
        return View(data);
    }

}