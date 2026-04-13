using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SafeVoice.Data;
using SafeVoice.Models;

namespace SafeVoice
{
    public class SupportLocationsController : Controller
    {
        private readonly AppDbContext _context;

        public SupportLocationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SupportLocations
        public async Task<IActionResult> Index()
        {
            return View(await _context.SupportLocations.ToListAsync());
        }

        // GET: SupportLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportLocation = await _context.SupportLocations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supportLocation == null)
            {
                return NotFound();
            }

            return View(supportLocation);
        }

        // GET: SupportLocations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SupportLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Address,Phone,Email,Website,Latitude,Longitude,OpeningHours,Description,Services,Is24Hour,IsEmergency,IsActive,CreatedAt,StationCode,District")] SupportLocation supportLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supportLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supportLocation);
        }

        // GET: SupportLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportLocation = await _context.SupportLocations.FindAsync(id);
            if (supportLocation == null)
            {
                return NotFound();
            }
            return View(supportLocation);
        }

        // POST: SupportLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Address,Phone,Email,Website,Latitude,Longitude,OpeningHours,Description,Services,Is24Hour,IsEmergency,IsActive,CreatedAt,StationCode,District")] SupportLocation supportLocation)
        {
            if (id != supportLocation.Id)
            {
                return NotFound();
            }

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
            return View(supportLocation);
        }

        // GET: SupportLocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportLocation = await _context.SupportLocations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supportLocation == null)
            {
                return NotFound();
            }

            return View(supportLocation);
        }

        // POST: SupportLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supportLocation = await _context.SupportLocations.FindAsync(id);
            if (supportLocation != null)
            {
                _context.SupportLocations.Remove(supportLocation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportLocationExists(int id)
        {
            return _context.SupportLocations.Any(e => e.Id == id);
        }
    }
}
