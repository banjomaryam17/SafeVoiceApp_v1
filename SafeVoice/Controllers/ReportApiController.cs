using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeVoice.Data;
using SafeVoice.Models;

namespace SafeVoice.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ReportsApiController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodaysReports([FromQuery] string apiKey)
        {
            var validKey = _configuration["ApiSettings:AdminApiKey"];
            if (string.IsNullOrEmpty(apiKey) || apiKey != validKey)
                return Unauthorized(new { message = "Invalid or missing API key." });

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var reports = await _context.Reports
                .Where(r => r.DateSubmitted >= today && r.DateSubmitted < tomorrow)
                .Select(r => new
                {
                    r.Id,
                    r.Description,
                    r.Location,
                    Status = r.Status.ToString(),
                    Priority = r.Priority.ToString(),
                    ReportingAs = r.ReportingAs.ToString(),
                    r.DateSubmitted,
                    r.IsHighPriority,
                    r.RequiresGardaAttention,
                    r.VictimAge,
                    IsAnonymous = r.SubmittedByUserId == null
                })
                .OrderByDescending(r => r.DateSubmitted)
                .ToListAsync();

            return Ok(new
            {
                date = today.ToString("yyyy-MM-dd"),
                totalReports = reports.Count,
                reports
            });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReports([FromQuery] string apiKey)
        {
            var validKey = _configuration["ApiSettings:AdminApiKey"];
            if (string.IsNullOrEmpty(apiKey) || apiKey != validKey)
                return Unauthorized(new { message = "Invalid or missing API key." });

            var reports = await _context.Reports
                .Select(r => new
                {
                    r.Id,
                    r.Description,
                    r.Location,
                    Status = r.Status.ToString(),
                    Priority = r.Priority.ToString(),
                    ReportingAs = r.ReportingAs.ToString(),
                    r.DateSubmitted,
                    r.DateReviewed,
                    r.DateResolved,
                    r.IsHighPriority,
                    r.RequiresGardaAttention,
                    r.VictimAge,
                    IsAnonymous = r.SubmittedByUserId == null
                })
                .OrderByDescending(r => r.DateSubmitted)
                .ToListAsync();

            return Ok(new
            {
                totalReports = reports.Count,
                reports
            });
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingReports([FromQuery] string apiKey)
        {
            var validKey = _configuration["ApiSettings:AdminApiKey"];
            if (string.IsNullOrEmpty(apiKey) || apiKey != validKey)
                return Unauthorized(new { message = "Invalid or missing API key." });

            var reports = await _context.Reports
                .Where(r => r.Status == ReportStatus.Pending)
                .Select(r => new
                {
                    r.Id,
                    r.Description,
                    r.Location,
                    Status = r.Status.ToString(),
                    Priority = r.Priority.ToString(),
                    ReportingAs = r.ReportingAs.ToString(),
                    r.DateSubmitted,
                    r.IsHighPriority,
                    r.RequiresGardaAttention,
                    IsAnonymous = r.SubmittedByUserId == null
                })
                .OrderByDescending(r => r.DateSubmitted)
                .ToListAsync();

            return Ok(new
            {
                totalPending = reports.Count,
                reports
            });
        }
    }
}