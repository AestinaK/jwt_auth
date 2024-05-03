using System.Security.Claims;
using jwt.Data;
using jwt.Models;

namespace jwt.Manager;

public interface IUserManager
{
    User CurrentUser();
}

public class UserManager:IUserManager
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _accessor;

    public UserManager(ApplicationDbContext context,
        IHttpContextAccessor accessor)
    {
        _context = context;
        _accessor = accessor;
    }
    public User CurrentUser()
    {
        var identity = _accessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new User
                {
                    Name = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                    Email = userClaims.FirstOrDefault(x=> x.Type == ClaimTypes.Email)?.Value,
                    Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        
    }
}