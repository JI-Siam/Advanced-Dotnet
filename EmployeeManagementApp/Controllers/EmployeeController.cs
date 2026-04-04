using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementApp.Models;
using EmployeeManagementApp.EF;
public class EmployeeController : Controller


{
    EmployeeDbContext db;

    public EmployeeController(EmployeeDbContext db)
    {
        this.db = db;
    }

    public IActionResult Index()
    {
        var data = db.Employees.ToList();
        return View(data);
    }

    public IActionResult Excellent()
    {
        var data = (from emp in db.Employees
                    where emp.PerformanceScore >= 4.5m
                    && emp.LowestProjectRating > 3.7m
                    && emp.TotalProjectsCompleted >= 6
                    select emp).ToList();

        foreach (var emp in data)
        {
            Console.WriteLine(emp.EmployeeName);
        }
        return View(data);
    }


}