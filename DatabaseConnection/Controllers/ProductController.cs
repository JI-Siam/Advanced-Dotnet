using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DatabaseConnection.Models;
using System.Data;
public class ProductController : Controller
{
    MyAppDbContext db;

    public ProductController(MyAppDbContext db)
    {
        this.db = db;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // add product to the db. 
        var data = db.Categories.ToList();
        return View(data);
    }

    [HttpPost]
    public IActionResult Index(Product pd)
    {
        // add product to the db. 

        try
        {
            db.Products.Add(pd);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        catch
        {
            TempData["msg"] = "And Unknown Error Occured";
            return View();
        }

    }

    public IActionResult List()
    {
        var data = db.Products.Include(p => p.Category).ToList();
        Console.WriteLine(data);
        return View(data);
    }


}