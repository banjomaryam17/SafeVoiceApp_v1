using Microsoft.AspNetCore.Mvc;

namespace SafeVoice.Controllers;

public class SupportController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}