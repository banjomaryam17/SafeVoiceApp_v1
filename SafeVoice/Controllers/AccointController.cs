using Microsoft.AspNetCore.Mvc;

namespace SafeVoice.Controllers;

public class AccointController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}