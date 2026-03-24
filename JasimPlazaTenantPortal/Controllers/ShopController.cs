using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jasim_Plaza_Tenant_Portal.Models;
using Jasim_Plaza_Tenant_Portal.EF.Tables;

public class ShopController : Controller
{
    //     Routes:
    // /Shop/Index
    // /Shop/Create
    // /Shop/Edit/{id}
    // /Shop/Delete/{id}
    // Tasks:
    // Add new shops
    // Assign owner to shop
    // Edit/Delete shops
    // In View:
    // Display all shops using foreach
    // Show owner name

    // ✔ Concepts Practiced:

    // CRUD operations
    // Model binding
    // Controller-to-view data flow

    JasimPlazaDbContext db;
    public ShopController(JasimPlazaDbContext db)
    {
        this.db = db;
    }
    public IActionResult Index()
    {
        var shops = db.Shops.Include(o => o.Owner).ToList();
        return View(shops);

    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Create(Shop shop)

    {
        try
        {
            db.Shops.Add(shop);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        catch
        {
            TempData["msg"] = "An Unknown Error Occured";
            return View(shop);
        }

    }


}