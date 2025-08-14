using Microsoft.AspNetCore.Mvc;

namespace PersonalExpenses.Controllers;

public class AboutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}