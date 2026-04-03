using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jasim_Plaza_Tenant_Portal.Models;
using Jasim_Plaza_Tenant_Portal.EF.Tables;

public class UserController : Controller
{
    private readonly JasimPlazaDbContext db;

    public UserController(JasimPlazaDbContext db)

    {
        this.db = db;
    }

    public IActionResult Index()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login");
        }

        var users = db.Users.OrderBy(u => u.Role).ThenBy(u => u.Name).ToList();
        ViewBag.CurrentUser = currentUser;
        return View(users);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        if (GetCurrentUser() != null)
        {
            return RedirectToAction("Index", "Home");
        }

        var model = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
        if (user == null)
        {
            TempData["msg"] = "Invalid email or password.";
            return View(model);
        }

        HttpContext.Session.SetInt32("CurrentUserId", user.Id);
        TempData["msg"] = $"Welcome, {user.Name}.";

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("CurrentUserId");
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Profile()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login");
        }

        var model = new UserProfileUpdateModel
        {
            Name = currentUser.Name,
            Password = currentUser.Password
        };

        ViewBag.CurrentUser = currentUser;
        return View(model);
    }

    [HttpPost]
    public IActionResult Profile(UserProfileUpdateModel model)
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.CurrentUser = currentUser;
            return View(model);
        }

        currentUser.Name = model.Name.Trim();
        currentUser.Password = model.Password;
        db.SaveChanges();

        TempData["msg"] = "Profile updated successfully.";
        return RedirectToAction("Profile");
    }

    private User? GetCurrentUser()
    {
        var sessionUserId = HttpContext.Session.GetInt32("CurrentUserId");
        return sessionUserId.HasValue
            ? db.Users.FirstOrDefault(u => u.Id == sessionUserId.Value)
            : null;
    }

}