using Microsoft.AspNetCore.Mvc;

namespace jwt.Controllers;

public class LoginController : Controller
{
    // GET
    public IActionResult Login()
    {
        return View();
    }
}