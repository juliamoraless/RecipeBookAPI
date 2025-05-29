using Microsoft.AspNetCore.Mvc;

namespace Application;

public class Controller1 : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}