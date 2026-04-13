using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SafeVoice.Data;
using SafeVoice.Models;

namespace SafeVoice.Controllers
{
    [Authorize] 
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize] // Require login
        public async Task<IActionResult> Index()
        {
            IQueryable<Report> reportsQuery;
    
            // Role-based access control
            if (User.IsInRole("SuperAdmin") || User.IsInRole("Garda") || User.IsInRole("SocialServices") || User.IsInRole("Moderator"))
            {
                // Admins can see ALL reports
                reportsQuery = _context.Reports.Include(r => r.SubmittedByUser);
            }
            else
            {
                // Regular users can ONLY see their own reports
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                reportsQuery = _context.Reports
                    .Include(r => r.SubmittedByUser)
                    .Where(r => r.SubmittedByUserId == userId);
            }

            var reports = await reportsQuery
                .OrderByDescending(r => r.DateSubmitted)
                .ToListAsync();

            return View(reports);
        }
        // GET: ReportController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: ReportController/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,ReportingAs,VictimName,VictimAge,RelationshipToVictim,ReporterName,ReporterContact,Location,Latitude,Longitude")] Report report)
        {
            if (ModelState.IsValid)
            {
                report.DateSubmitted = DateTime.Now;
        
                // Link to logged-in user if authenticated
                if (User.Identity.IsAuthenticated)
                {
                    report.SubmittedByUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                }
        
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // GET: ReportController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        // POST: ReportController/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,ReportingAs,VictimName,VictimAge,RelationshipToVictim,ReporterName,ReporterContact,Location,Latitude,Longitude,Status,DateSubmitted")] Report report)
        {
            if (id != report.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // GET: ReportController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: ReportController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report != null)
            {
                _context.Reports.Remove(report);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}
