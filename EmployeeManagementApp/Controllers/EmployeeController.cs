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
        TempData["CreateMsg"] = "Employee with Id " + emp.EmployeeId + " created successfully";
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Update(int id)
    {
        var data = db.Employees.Find(id);
        return View(data);
    }

    [HttpPost]
    public IActionResult Update(Employee formObj)
    {
        try
        {
            db.Employees.Update(formObj);
            db.SaveChanges();
            TempData["EditMsg"] = "Employee with Id " + formObj.EmployeeId + " Updated successfully";
            return RedirectToAction("Index");
        }

        catch
        {
            TempData["EditMsg"] = "Employee with Id " + formObj.EmployeeId + " Could not be updated - Unknown Error!!";
            return View(formObj);
        }

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