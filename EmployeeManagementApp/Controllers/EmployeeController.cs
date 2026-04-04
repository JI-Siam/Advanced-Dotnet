using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementApp.Models;
using EmployeeManagementApp.EF;
using EmployeeManagementApp.EF.Tables;
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


    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Employee emp)
    {

        db.Employees.Add(emp);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var employee = db.Employees.Find(id);
        db.Employees.Remove(employee);
        db.SaveChanges();
        TempData["deleteMsg"] = "Employee with Id " + id + " deleted successfully";
        return RedirectToAction("Index");
    }

    public IActionResult Details(int id)
    {
        var employee = db.Employees.Find(id);
        return View(employee);
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