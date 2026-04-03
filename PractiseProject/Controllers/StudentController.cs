using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PractiseProject.EF;
using PractiseProject.EF.Tables;
using PractiseProject.Models;

namespace PractiseProject.Controllers;

public class StudentController : Controller
{
    PractiseProjectDbContext db;
    public StudentController(PractiseProjectDbContext db)
    {
        this.db = db;
    }

    public IActionResult Index()
    {
        var studentData = db.Students.ToList();
        return View(studentData);
    }


}