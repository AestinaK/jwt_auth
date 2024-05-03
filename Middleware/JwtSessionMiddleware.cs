using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace jwt.Middleware;

public class JwtSessionMiddleware
{
    private readonly IConfiguration _config;
    private readonly RequestDelegate _next;

    public JwtSessionMiddleware(IConfiguration config,
        RequestDelegate next)
    {
        _config = config;
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        var tokenString = context.Session.GetString("Jwt_token");

        if (!string.IsNullOrEmpty(tokenString))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:key"]);

            try
            {
                tokenHandler.ValidateToken(tokenString, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                // Attach user principal to context if token is valid
                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    var claims = new ClaimsIdentity(jwtToken.Claims, "jwt");
                    context.User = new ClaimsPrincipal(claims);
                }
            }
            catch (Exception ex)
            {
                // Handle token validation failure (e.g., token expired)
                context.Session.Remove("Jwt_token");
            }
        }
        await _next(context);
    }
}