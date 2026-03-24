using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jasim_Plaza_Tenant_Portal.Models;
using Jasim_Plaza_Tenant_Portal.EF.Tables;

public class RentController : Controller
{
    //     /Rent/Index
    // /Rent/Create
    // /Rent/Details/{id}
    // Tasks:
    // Add monthly rent record for each shop
    // Track:
    // Month
    // Year
    // Status (Paid / Pending / Due)
    JasimPlazaDbContext db;
    public RentController(JasimPlazaDbContext db)
    {
        this.db = db;
    }

    public IActionResult Index()
    {
        var records = db.RentRecords.Include(s => s.Shop).ToList();
        return View(records);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var shops = db.Shops.ToList();
        return View(shops);
    }

    [HttpPost]
    public IActionResult Create(RentRecord rec)
    {
        try
        {
            db.Add(rec);
            db.SaveChanges();
            TempData["msg"] = "Record Created Successfully";
            return RedirectToAction("Index");
        }
        catch
        {
            TempData["msg"] = "An Unknown Error Occured";
            return View();
        }

    }


    public IActionResult Details(int id)
    {

        var record = db.RentRecords.Find(id);
        return View(record);
    }
}