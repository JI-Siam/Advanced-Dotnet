using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormValidation.Models;

namespace FormValidation.Controllers;



public class RegistrationController : Controller
{

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(RegistrationModel obj)
    {
        if (ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }
        return View(obj);
    }

}