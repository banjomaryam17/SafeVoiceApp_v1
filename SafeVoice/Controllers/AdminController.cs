using System.Security.Claims;
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

    [Authorize(Roles = "Moderator,Garda,SocialServices,SuperAdmin")]
    public async Task<IActionResult> ManageReports()
    {
        var reports = await _context.Reports
            .Include(r => r.SubmittedByUser)
            .Include(r => r.ReviewedByUser)
            .OrderByDescending(r => r.DateSubmitted)
            .ToListAsync();
        return View(reports);
    }
    // API: Approve Report
    [HttpPost]
    [Route("api/admin/approve/{id}")]
    [Authorize(Roles = "Garda,SocialServices,SuperAdmin")]
    public async Task<IActionResult> ApiApproveReport(int id)
    {
        var report = await _context.Reports.FindAsync(id);
        if (report == null) 
            return NotFound(new { message = "Report not found" });

        report.Status = ReportStatus.Accepted;
        report.DateReviewed = DateTime.Now;
        report.ReviewedByUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    
        await _context.SaveChangesAsync();
    
        return Ok(new { 
            message = "Report approved successfully",
            reportId = id,
            status = "Accepted",
            reviewedAt = report.DateReviewed
        });
    }

// API: Reject Report
    [HttpPost]
    [Route("api/admin/reject/{id}")]
    [Authorize(Roles = "Garda,SocialServices,SuperAdmin")]
    public async Task<IActionResult> ApiRejectReport(int id)
    {
        var report = await _context.Reports.FindAsync(id);
        if (report == null) 
            return NotFound(new { message = "Report not found" });

        report.Status = ReportStatus.Rejected;
        report.DateReviewed = DateTime.Now;
        report.ReviewedByUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    
        await _context.SaveChangesAsync();
    
        return Ok(new { 
            message = "Report rejected",
            reportId = id,
            status = "Rejected",
            reviewedAt = report.DateReviewed
        });
    }

// API: Get Report Status
    [HttpGet]
    [Route("api/reports/{id}/status")]
    public async Task<IActionResult> GetReportStatus(int id)
    {
        var report = await _context.Reports
            .Include(r => r.ReviewedByUser)
            .FirstOrDefaultAsync(r => r.Id == id);
    
        if (report == null)
            return NotFound();

        return Ok(new {
            reportId = id,
            status = report.Status.ToString(),
            dateSubmitted = report.DateSubmitted,
            dateReviewed = report.DateReviewed,
            reviewedBy = report.ReviewedByUser?.Username
        });
    }
}

