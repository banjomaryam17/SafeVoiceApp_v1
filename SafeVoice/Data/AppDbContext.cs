using Microsoft.EntityFrameworkCore;
using SafeVoice.Models;

namespace SafeVoice.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }

    public DbSet<Report> Reports { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<SupportLocation> SupportLocations { get; set; }

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

        modelBuilder.Entity<SupportLocation>()
            .Property(s => s.Type)
            .HasConversion<string>();

        // Seed some Irish support locations
        modelBuilder.Entity<SupportLocation>().HasData(
            new SupportLocation
            {
                Id = 1,
                Name = "Dundalk Garda Station",
                Type = SupportLocationType.GardaStation,
                Address = "Park Street, Dundalk, Co. Louth",
                Phone = "042-9388400",
                Latitude = 53.9994,
                Longitude = -6.4019,
                StationCode = "DUN",
                District = "Louth",
                OpeningHours = "24/7",
                Is24Hour = true,
                IsEmergency = true,
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new SupportLocation
            {
                Id = 2,
                Name = "Tusla Dundalk",
                Type = SupportLocationType.Tusla,
                Address = "Crowe Street, Dundalk, Co. Louth",
                Phone = "042-9394700",
                Email = "dundalk@tusla.ie",
                Latitude = 54.0014,
                Longitude = -6.4050,
                OpeningHours = "Mon-Fri 9:00-17:00",
                Description = "Child and Family Agency services",
                Services = "Child protection, family support, welfare services",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new SupportLocation
            {
                Id = 3,
                Name = "HSE Louth Community Services",
                Type = SupportLocationType.HSE,
                Address = "Stapleton Place, Dundalk, Co. Louth",
                Phone = "041-6850000",
                Latitude = 54.0041,
                Longitude = -6.3981,
                OpeningHours = "Mon-Fri 9:00-17:00",
                Description = "Health and social care services",
                Services = "Mental health, community care, counselling",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new SupportLocation
            {
                Id = 4,
                Name = "Childline",
                Type = SupportLocationType.Helpline,
                Address = "National Service",
                Phone = "1800-666666",
                Website = "https://childline.ie",
                Latitude = 53.3498,
                Longitude = -6.2603, // Dublin coordinates
                OpeningHours = "24/7",
                Is24Hour = true,
                Description = "Free 24/7 listening service for children",
                Services = "Confidential support for children and young people",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new SupportLocation
            {
                Id = 5,
                Name = "Our Lady of Lourdes Hospital",
                Type = SupportLocationType.Hospital,
                Address = "Drogheda, Co. Louth",
                Phone = "041-9874000",
                Latitude = 53.7175,
                Longitude = -6.3477,
                OpeningHours = "24/7 Emergency",
                Is24Hour = true,
                IsEmergency = true,
                Description = "Emergency medical services",
                Services = "Emergency department, paediatric care",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1)
            }
        );
    }
}