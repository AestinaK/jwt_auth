using AspNetCoreHero.ToastNotification.Abstractions;
using jwt.Data;
using jwt.Models;
using jwt.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace jwt.Controllers;

public class UserRegisterController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notyfService;

    public UserRegisterController( ApplicationDbContext context,
        INotyfService notyfService)
    {
        _context = context;
        _notyfService = notyfService;
    }

    // GET
    [HttpGet]
    public IActionResult Add()
    {
        var vm = new UserVm();
        return View(vm);
    }

    [HttpPost]
    public IActionResult Add(UserVm vm)
    {
        try
        {
            var user = new User()
            {
                Name = vm.Name,
                Email = vm.Email,
                Role = vm.Role,
                Password = BCrypt.Net.BCrypt.HashPassword(vm.Password)
            };
            _context.user.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login","Login");
        }
        catch (Exception e)
        {
           _notyfService.Error(e.Message);
        }

        return View(vm);
    }

   
}