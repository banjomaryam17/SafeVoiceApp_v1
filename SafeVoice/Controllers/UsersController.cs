using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeVoice.Data;
using SafeVoice.Models;
using System.Security.Claims;

namespace SafeVoice.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class UsersController : Controller
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Users
    public async Task<IActionResult> Index()
    {
        var users = await _context.Users
            .Include(u => u.Reports)
            .ToListAsync();
        return View(users);
    }

    // GET: Users/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var user = await _context.Users
            .Include(u => u.Reports)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (user == null) return NotFound();

        return View(user);
    }

    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        return View(user);
    }

    // POST: Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, User user)
    {
        if (id != user.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser == null) return NotFound();

                // Update allowed fields only (don't update password hash)
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Role = user.Role;
                existingUser.IsActive = user.IsActive;
                existingUser.BadgeNumber = user.BadgeNumber;
                existingUser.Department = user.Department;

                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = $"User {existingUser.Username} updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    // POST: Users/PromoteRole/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PromoteRole(int id, UserRole newRole)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        var oldRole = user.Role;
        user.Role = newRole;
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"User {user.Username} role changed from {oldRole} to {newRole}.";
        return RedirectToAction(nameof(Index));
    }

    // POST: Users/ToggleStatus/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        // Don't allow deactivating yourself
        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (user.Id == currentUserId)
        {
            TempData["ErrorMessage"] = "You cannot deactivate your own account.";
            return RedirectToAction(nameof(Index));
        }

        user.IsActive = !user.IsActive;
        await _context.SaveChangesAsync();

        var status = user.IsActive ? "activated" : "deactivated";
        TempData["SuccessMessage"] = $"User {user.Username} has been {status}.";
        
        return RedirectToAction(nameof(Index));
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}