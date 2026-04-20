using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeVoice.Data;
using SafeVoice.Models;
using System.Security.Claims;

namespace SafeVoice.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = "/")
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == model.Username && u.IsActive);

        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
        {
            ModelState.AddModelError("", "Invalid username or password");
            return View(model);
        }

        // Update last login
        user.LastLogin = DateTime.Now;
        await _context.SaveChangesAsync();

        // Create claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("DisplayName", $"{user.FirstName} {user.LastName}".Trim()),
            new Claim("UserRole", user.Role.GetDisplayName())
        };

        // Add role-specific claims
        if (!string.IsNullOrEmpty(user.BadgeNumber))
            claims.Add(new Claim("BadgeNumber", user.BadgeNumber));
        
        if (!string.IsNullOrEmpty(user.Department))
            claims.Add(new Claim("Department", user.Department));

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = model.RememberMe,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return LocalRedirect(returnUrl);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Check if username or email already exists
        if (await _context.Users.AnyAsync(u => u.Username == model.Username))
        {
            ModelState.AddModelError("Username", "Username is already taken");
            return View(model);
        }

        if (await _context.Users.AnyAsync(u => u.Email == model.Email))
        {
            ModelState.AddModelError("Email", "Email is already registered");
            return View(model);
        }

        var user = new User
        {
            Username = model.Username,
            Email = model.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            Role = UserRole.Regular, // All new registrations are regular users
            FirstName = model.FirstName,
            LastName = model.LastName,
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Auto-login after registration
        return RedirectToAction("Login");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public IActionResult Profile()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var user = _context.Users.Find(userId);
        
        if (user == null)
            return NotFound();

        return View(user);
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    // External Login (Google/Microsoft)
    [AllowAnonymous]
    [HttpPost]
    public IActionResult ExternalLogin(string provider, string returnUrl = null)
    {
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
        var properties = new AuthenticationProperties 
        { 
            RedirectUri = redirectUrl,
            Items = { { "scheme", provider } }
        };
        return Challenge(properties, provider);
    }

    // External Login Callback
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
    {
        // Get the external login info
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        if (result?.Succeeded != true)
        {
            TempData["ErrorMessage"] = "Error loading external login information.";
            return RedirectToAction(nameof(Login));
        }

        // Get user info from external provider
        var externalUser = result.Principal;
        var email = externalUser.FindFirst(ClaimTypes.Email)?.Value;
        var name = externalUser.FindFirst(ClaimTypes.Name)?.Value;
        var givenName = externalUser.FindFirst(ClaimTypes.GivenName)?.Value;
        var surname = externalUser.FindFirst(ClaimTypes.Surname)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            TempData["ErrorMessage"] = "Unable to get email from external provider.";
            return RedirectToAction(nameof(Login));
        }

        // Check if user already exists
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            // Create new user from external login
            var username = email.Split('@')[0]; // Use email prefix as username
            
            // Make sure username is unique
            var existingUsername = await _context.Users.AnyAsync(u => u.Username == username);
            if (existingUsername)
            {
                username = $"{username}_{new Random().Next(1000, 9999)}";
            }

            user = new User
            {
                Username = username,
                Email = email,
                FirstName = givenName ?? name?.Split(' ')[0] ?? "User",
                LastName = surname ?? (name?.Split(' ').Length > 1 ? string.Join(" ", name.Split(' ').Skip(1)) : ""),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()), // Random password for external logins
                Role = UserRole.Regular,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // Update last login
        user.LastLogin = DateTime.Now;
        await _context.SaveChangesAsync();

        // Create claims for our application
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("DisplayName", $"{user.FirstName} {user.LastName}".Trim()),
            new Claim("UserRole", user.Role.GetDisplayName())
        };

        // Add role-specific claims
        if (!string.IsNullOrEmpty(user.BadgeNumber))
            claims.Add(new Claim("BadgeNumber", user.BadgeNumber));
        
        if (!string.IsNullOrEmpty(user.Department))
            claims.Add(new Claim("Department", user.Department));

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToLocal(returnUrl);
    }

    // Helper method for redirects
    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        
        return RedirectToAction("Index", "Home");
    }
    
}