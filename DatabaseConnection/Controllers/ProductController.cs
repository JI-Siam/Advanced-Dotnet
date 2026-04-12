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
        // show the categories names inorder to input product
        var data = db.Categories.ToList();
        return View(data);
    }

    [HttpPost]
    public IActionResult Index(ProductDTO pd)
    {
        // add product to the db. 

        if (ModelState.IsValid)
        {
            Product product = new Product()
            {
                Name = pd.Name,

            };
            db.Products.Add(pd);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        TempData["msg"] = "And Unknown Error Occured";
        return View();


    }

    public IActionResult List()
    {
        var data = db.Products.Include(p => p.Category).ToList();
        Console.WriteLine(data);
        return View(data);
    }


}