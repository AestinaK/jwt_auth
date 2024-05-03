using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AspNetCoreHero.ToastNotification.Abstractions;
using jwt.Data;
using jwt.Models;
using jwt.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace jwt.Controllers;

public class LoginController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;
    private readonly INotyfService _notyfService;

    public LoginController(ApplicationDbContext context,
        IConfiguration config,
        INotyfService notyfService)
    {
        _context = context;
        _config = config;
        _notyfService = notyfService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        var vm = new LoginVm();
        return View(vm);
    }
    // GET
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login(LoginVm vm)
    {
        try
        {
            var user = Authenticate(vm);
            if (user != null)
            {
                GenerateToken(user);
                _notyfService.Success("Login Successfully!");
                return RedirectToAction("Index", "Home");
            }
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
        }
        return View(vm);
    }

    //To generate Token
    private string GenerateToken(User user)
    {
        var tokenHandeler = new JwtSecurityTokenHandler();
        string issuer = _config["Jwt:Issuer"];
        string audience = _config["Jwt:Audience"];
        string key = _config["Jwt:Key"];
        
        var secretKey = Encoding.UTF8.GetBytes(key);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Issuer = issuer,
            Audience = audience,
            Expires = DateTime.Now.AddMinutes(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandeler.CreateToken(tokenDescriptor);
        return tokenHandeler.WriteToken(token);
    }
    
    //To Authenticate User
    private User Authenticate(LoginVm vm)
    {
        var currentUser = _context.user.FirstOrDefault(x => x.Name.Equals(vm.UserName)
                                                            && x.Password == vm.Password);
        if (currentUser != null)
        {
            return currentUser;
            
        }
        return null;
    }
    
}