using Microsoft.EntityFrameworkCore;
using SafeVoice.Models;

namespace SafeVoice.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }

    public DbSet<Report> Reports { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User-Report relationships
        modelBuilder.Entity<Report>()
            .HasOne(r => r.SubmittedByUser)
            .WithMany(u => u.Reports)
            .HasForeignKey(r => r.SubmittedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Report>()
            .HasOne(r => r.ReviewedByUser)
            .WithMany()
            .HasForeignKey(r => r.ReviewedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure enum conversions
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Report>()
            .Property(r => r.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Report>()
            .Property(r => r.ReportingAs)
            .HasConversion<string>();

        modelBuilder.Entity<Report>()
            .Property(r => r.Priority)
            .HasConversion<string>();
    }
}