using Microsoft.EntityFrameworkCore;
using SafeVoice.Models;
namespace SafeVoice.Data;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }
    
    public DbSet<Report> Reports { get; set; }
}