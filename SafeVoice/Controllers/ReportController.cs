using Microsoft.AspNetCore.Mvc;

namespace SafeVoice.Controllers;

public class ReportController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}