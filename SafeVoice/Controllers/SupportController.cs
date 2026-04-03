using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeVoice.Data;
using SafeVoice.Models;
using System.Security.Claims;

namespace SafeVoice.Controllers;

public class SupportController : Controller
{
    private readonly AppDbContext _context;

    public SupportController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Support
    public async Task<IActionResult> Index()
    {
        var supportLocations = await _context.SupportLocations
            .Where(s => s.IsActive)
            .OrderBy(s => s.Type)
            .ThenBy(s => s.Name)
            .ToListAsync();

        return View(supportLocations);
    }

    // GET: Support/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var supportLocation = await _context.SupportLocations
            .FirstOrDefaultAsync(s => s.Id == id);

        if (supportLocation == null)
            return NotFound();

        return View(supportLocation);
    }

    // GET: Support/Create
    [Authorize(Roles = "Moderator,Garda,SocialServices,SuperAdmin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Support/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Garda,SocialServices,SuperAdmin")]
    public async Task<IActionResult> Create([Bind("Name,Type,Address,Phone,Email,Website,Latitude,Longitude,OpeningHours,Description,Services,Is24Hour,IsEmergency,StationCode,District")] SupportLocation supportLocation)
    {
        if (ModelState.IsValid)
        {
            supportLocation.IsActive = true;
            supportLocation.CreatedAt = DateTime.Now;
            
            _context.Add(supportLocation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(supportLocation);
    }

    // GET: Support/Edit/5
    [Authorize(Roles = "Moderator,Garda,SocialServices,SuperAdmin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var supportLocation = await _context.SupportLocations.FindAsync(id);
        if (supportLocation == null)
            return NotFound();

        return View(supportLocation);
    }

    // POST: Support/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Moderator,Garda,SocialServices,SuperAdmin")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Address,Phone,Email,Website,Latitude,Longitude,OpeningHours,Description,Services,Is24Hour,IsEmergency,StationCode,District,IsActive")] SupportLocation supportLocation)
    {
        if (id != supportLocation.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(supportLocation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupportLocationExists(supportLocation.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(supportLocation);
    }

    // GET: Support/Delete/5
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var supportLocation = await _context.SupportLocations
            .FirstOrDefaultAsync(s => s.Id == id);

        if (supportLocation == null)
            return NotFound();

        return View(supportLocation);
    }

    // POST: Support/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var supportLocation = await _context.SupportLocations.FindAsync(id);
        if (supportLocation != null)
        {
            // Soft delete - just mark as inactive
            supportLocation.IsActive = false;
            _context.Update(supportLocation);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // API: Get nearest support locations (JSON endpoint)
    [HttpGet]
    public async Task<IActionResult> GetNearest(double latitude, double longitude, int maxResults = 5, string? type = null)
    {
        var query = _context.SupportLocations.Where(s => s.IsActive);

        // Filter by type if specified
        if (!string.IsNullOrEmpty(type) && Enum.TryParse<SupportLocationType>(type, out var locationType))
        {
            query = query.Where(s => s.Type == locationType);
        }

        var locations = await query.ToListAsync();

        // Calculate distances and sort by nearest
        var locationsWithDistance = locations.Select(location => new
        {
            location,
            distance = CalculateDistance(latitude, longitude, location.Latitude, location.Longitude)
        })
        .OrderBy(x => x.distance)
        .Take(maxResults)
        .Select(x => new
        {
            id = x.location.Id,
            name = x.location.Name,
            type = x.location.Type.GetDisplayName(),
            typeIcon = x.location.Type.GetIconClass(),
            address = x.location.Address,
            phone = x.location.Phone,
            email = x.location.Email,
            website = x.location.Website,
            latitude = x.location.Latitude,
            longitude = x.location.Longitude,
            openingHours = x.location.OpeningHours,
            description = x.location.Description,
            services = x.location.Services,
            is24Hour = x.location.Is24Hour,
            isEmergency = x.location.IsEmergency,
            distance = Math.Round(x.distance, 2)
        })
        .ToList();

        return Json(locationsWithDistance);
    }

    // GET: Support/Map - Interactive map view
    public IActionResult Map()
    {
        return View();
    }

    // GET: Support/Emergency - Quick access to emergency services
    public async Task<IActionResult> Emergency()
    {
        var emergencyServices = await _context.SupportLocations
            .Where(s => s.IsActive && (s.IsEmergency || s.Is24Hour))
            .OrderBy(s => s.Type)
            .ToListAsync();

        return View(emergencyServices);
    }

    private bool SupportLocationExists(int id)
    {
        return _context.SupportLocations.Any(e => e.Id == id);
    }

    // Haversine formula to calculate distance between two points
    private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var R = 6371; // Earth's radius in kilometers
        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double ToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }
}
