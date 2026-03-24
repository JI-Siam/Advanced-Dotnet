using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jasim_Plaza_Tenant_Portal.Models;
using Jasim_Plaza_Tenant_Portal.EF.Tables;
public class UserController : Controller
{
    JasimPlazaDbContext db;
    public UserController(JasimPlazaDbContext db)

    {
        this.db = db;
    }

    public IActionResult RentRecords()
    {
        var records = db.RentRecords.Include(s => s.Shop).ToList();
        return View(records);
    }


}