using Microsoft.AspNetCore.Mvc;

namespace SafeVoice.Controllers;

public class AdminController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}