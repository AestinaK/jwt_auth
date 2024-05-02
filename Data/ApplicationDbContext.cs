using jwt.Models;
using Microsoft.EntityFrameworkCore;
namespace jwt.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> user { get; set; }
}