using System.Text;
using jwt.Data;
using jwt.Models;
using jwt.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace jwt.Controllers;

public class LoginController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public LoginController(ApplicationDbContext context,
        IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    // GET
    public IActionResult Login()
    {
        return View();
    }

    //To generate Token
    // private string GenerateToken(User user)
    // {
    //     var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
    //     var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    //     var claims = new[]
    // }
    
    //To Authenticate User
    private User Authenticate(LoginVm vm)
    {
        var currentUser = _context.user.FirstOrDefault(x => x.Name.ToLower() == vm.UserName.ToLower()
                                                            && x.Password == vm.Password);
        if (currentUser != null)
        {
            return currentUser;
            
        }
        return null;
    }
}