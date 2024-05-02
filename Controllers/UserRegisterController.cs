using jwt.Data;
using jwt.Models;
using jwt.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace jwt.Controllers;

public class UserRegisterController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserRegisterController( ApplicationDbContext context)
    {
        _context = context;
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
                Password = vm.Password
            };
            _context.user.Add(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Add));
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}