using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeVoice.Controllers;
using SafeVoice.Data;
using SafeVoice.Models;
using System.Security.Claims;
using Xunit;

namespace SafeVoice.Tests;

public class ReportControllerTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    // Helper: creates a controller with a fake logged-in user
    private ReportController CreateController(AppDbContext context, string userId = "1", string? role = null)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, "testuser")
        };

        if (role != null)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var identity = new ClaimsIdentity(claims, "mock");
        var user = new ClaimsPrincipal(identity);

        var controller = new ReportController(context);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        return controller;
    }

    [Fact]
    public async Task Index_ReturnsViewWithReports()
    {
        using var context = GetInMemoryDbContext();
        var controller = CreateController(context, userId: "1");

        var result = await controller.Index();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_ValidReport_ReturnsRedirectToIndex()
    {
        using var context = GetInMemoryDbContext();
        var controller = CreateController(context, userId: "1");

        var report = new Report
        {
            Description = "Test incident description",
            ReportingAs = ReportingAs.Myself,
            Location = "Test Location"
        };

        var result = await controller.Create(report);

        Assert.IsType<RedirectToActionResult>(result);
        var redirectResult = result as RedirectToActionResult;
        Assert.Equal("Index", redirectResult!.ActionName);
    }
}