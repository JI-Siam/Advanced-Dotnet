using Microsoft.AspNetCore.Mvc;

public class LearnerController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(Learner obj)
    {
        string err = " ";
        if (ModelState.IsValid)
        {
            Console.WriteLine(obj.Email.Split('@')[0]);
            if (obj.Email.Split('@')[0] == obj.Id)
            {
                TempData["Msg"] = "Registration was successful";
                return RedirectToAction("Login");
            }
            err = "Id and Email don't match";
        }
        ViewBag.err = err;
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

}