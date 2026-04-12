using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DatabaseConnection.Models;
using System.Data;
public class CategoryController : Controller
{
    MyAppDbContext db;
    public CategoryController(MyAppDbContext db)
    {
        this.db = db;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(CategoryDTO ct)
    {

        if (ModelState.IsValid)
        {
            Category category = new Category()
            {
                Name = ct.Name
            };
            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("List");

        }

        TempData["msg"] = "an Unknown Error Occured";
        return View();


    }

    public IActionResult List()
    {
        var data = db.Categories.ToList();
        return View(data);
    }


}