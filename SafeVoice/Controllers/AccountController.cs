using Microsoft.AspNetCore.Mvc;

namespace SafeVoice.Controllers;

public class AccountController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}