using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using jwt.Models;
using Microsoft.AspNetCore.Authorization;

namespace jwt.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpContextAccessor _accessor;

    public HomeController(ILogger<HomeController> logger,
        IHttpContextAccessor accessor)
    {
        _logger = logger;
        _accessor = accessor;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
