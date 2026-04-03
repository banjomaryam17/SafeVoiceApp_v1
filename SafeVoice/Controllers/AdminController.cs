using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeVoice.Data;
using SafeVoice.Models;

namespace SafeVoice.Controllers;

[Authorize(Roles = "SuperAdmin,Garda,SocialServices")]
public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var stats = new AdminDashboardViewModel
        {
            TotalReports = await _context.Reports.CountAsync(),
            PendingReports = await _context.Reports.CountAsync(r => r.Status == ReportStatus.Pending),
            TotalUsers = await _context.Users.CountAsync(),
            ActiveSupportLocations = await _context.SupportLocations.CountAsync(s => s.IsActive),
            RecentReports = await _context.Reports
                .Include(r => r.SubmittedByUser)
                .OrderByDescending(r => r.DateSubmitted)
                .Take(5)
                .ToListAsync()
        };

        return View(stats);
    }

    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Users()
    {
        var users = await _context.Users.OrderBy(u => u.Username).ToListAsync();
        return View(users);
    }

    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> ManageReports()
    {
        var reports = await _context.Reports
            .Include(r => r.SubmittedByUser)
            .Include(r => r.ReviewedByUser)
            .OrderByDescending(r => r.DateSubmitted)
            .ToListAsync();
        return View(reports);
    }
}

public class AdminDashboardViewModel
{
    public int TotalReports { get; set; }
    public int PendingReports { get; set; }
    public int TotalUsers { get; set; }
    public int ActiveSupportLocations { get; set; }
    public List<Report> RecentReports { get; set; } = new();
}