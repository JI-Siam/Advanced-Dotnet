using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jasim_Plaza_Tenant_Portal.Models;
using Jasim_Plaza_Tenant_Portal.EF.Tables;

namespace Jasim_Plaza_Tenant_Portal.Controllers;

public class HomeController : Controller
{
    private readonly JasimPlazaDbContext db;

    public HomeController(JasimPlazaDbContext db)
    {
        this.db = db;
    }

    public IActionResult Index()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "User", new { returnUrl = Request.Path + Request.QueryString });
        }

        var isAdmin = string.Equals(currentUser?.Role, "Admin", StringComparison.OrdinalIgnoreCase);

        ViewBag.CurrentUser = currentUser;
        ViewBag.IsAdmin = isAdmin;

        if (isAdmin)
        {
            var month = DateTime.Today.Month;
            var year = DateTime.Today.Year;

            var currentMonthRecords = db.RentRecords
                .Include(r => r.Shop)
                .Where(r => r.Month == month && r.Year == year);

            ViewBag.TotalShops = db.Shops.Count();
            ViewBag.TotalOwners = db.Users.Count(u => u.Role == "Owner");
            ViewBag.DueRecords = currentMonthRecords.Count(r => r.Status == "Due");
            ViewBag.PendingRecords = currentMonthRecords.Count(r => r.Status == "Pending");
            ViewBag.PaidRecords = currentMonthRecords.Count(r => r.Status == "Paid");
            ViewBag.ShopsWithDue = currentMonthRecords.Where(r => r.Status == "Due").Select(r => r.ShopId).Distinct().Count();
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private User? GetCurrentUser()
    {
        var sessionUserId = HttpContext.Session.GetInt32("CurrentUserId");
        return sessionUserId.HasValue
            ? db.Users.FirstOrDefault(u => u.Id == sessionUserId.Value)
            : null;
    }
}
