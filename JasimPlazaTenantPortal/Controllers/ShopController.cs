using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jasim_Plaza_Tenant_Portal.EF.Tables;

public class ShopController : Controller
{
    private readonly JasimPlazaDbContext db;

    public ShopController(JasimPlazaDbContext db)
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

        IQueryable<Shop> query = db.Shops.Include(o => o.Owner);
        if (!IsAdmin(currentUser))
        {
            query = query.Where(s => s.OwnerId == currentUser.Id);
        }

        var shops = query.OrderBy(s => s.ShopName).ToList();

        ViewBag.CurrentUser = currentUser;
        return View(shops);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var currentUser = GetCurrentUser();
        if (!IsAdmin(currentUser))
        {
            TempData["msg"] = "Only Admin can create shops.";
            return RedirectToAction("Index");
        }

        ViewBag.Owners = GetOwners();
        return View();
    }

    [HttpPost]
    public IActionResult Create(Shop shop)
    {
        var currentUser = GetCurrentUser();
        if (!IsAdmin(currentUser))
        {
            TempData["msg"] = "Only Admin can create shops.";
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Owners = GetOwners();
            return View(shop);
        }

        try
        {
            db.Shops.Add(shop);
            db.SaveChanges();
            TempData["msg"] = "Shop created successfully.";
            return RedirectToAction("Index");
        }
        catch
        {
            TempData["msg"] = "An unknown error occurred while creating the shop.";
            ViewBag.Owners = GetOwners();
            return View(shop);
        }

    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var currentUser = GetCurrentUser();
        if (!IsAdmin(currentUser))
        {
            TempData["msg"] = "Only Admin can edit shops.";
            return RedirectToAction("Index");
        }

        var shop = db.Shops.FirstOrDefault(s => s.Id == id);
        if (shop == null)
        {
            TempData["msg"] = "Shop not found.";
            return RedirectToAction("Index");
        }

        ViewBag.Owners = GetOwners();
        return View(shop);
    }

    [HttpPost]
    public IActionResult Edit(Shop shop)
    {
        var currentUser = GetCurrentUser();
        if (!IsAdmin(currentUser))
        {
            TempData["msg"] = "Only Admin can edit shops.";
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Owners = GetOwners();
            return View(shop);
        }

        var existingShop = db.Shops.FirstOrDefault(s => s.Id == shop.Id);
        if (existingShop == null)
        {
            TempData["msg"] = "Shop not found.";
            return RedirectToAction("Index");
        }

        existingShop.ShopName = shop.ShopName;
        existingShop.Location = shop.Location;
        existingShop.MonthlyRent = shop.MonthlyRent;
        existingShop.OwnerId = shop.OwnerId;
        db.SaveChanges();

        TempData["msg"] = "Shop updated successfully.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var currentUser = GetCurrentUser();
        if (!IsAdmin(currentUser))
        {
            TempData["msg"] = "Only Admin can delete shops.";
            return RedirectToAction("Index");
        }

        var shop = db.Shops.Include(s => s.Owner).FirstOrDefault(s => s.Id == id);
        if (shop == null)
        {
            TempData["msg"] = "Shop not found.";
            return RedirectToAction("Index");
        }

        return View(shop);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var currentUser = GetCurrentUser();
        if (!IsAdmin(currentUser))
        {
            TempData["msg"] = "Only Admin can delete shops.";
            return RedirectToAction("Index");
        }

        var shop = db.Shops.Include(s => s.RentRecords).FirstOrDefault(s => s.Id == id);
        if (shop == null)
        {
            TempData["msg"] = "Shop not found.";
            return RedirectToAction("Index");
        }

        if (shop.RentRecords.Any())
        {
            db.RentRecords.RemoveRange(shop.RentRecords);
        }

        db.Shops.Remove(shop);
        db.SaveChanges();
        TempData["msg"] = "Shop deleted successfully.";
        return RedirectToAction("Index");
    }

    private List<User> GetOwners()
    {
        return db.Users.Where(u => u.Role == "Owner").OrderBy(u => u.Name).ToList();
    }

    private User? GetCurrentUser()
    {
        var sessionUserId = HttpContext.Session.GetInt32("CurrentUserId");
        return sessionUserId.HasValue
            ? db.Users.FirstOrDefault(u => u.Id == sessionUserId.Value)
            : null;
    }

    private static bool IsAdmin(User? user)
    {
        return string.Equals(user?.Role, "Admin", StringComparison.OrdinalIgnoreCase);
    }

}