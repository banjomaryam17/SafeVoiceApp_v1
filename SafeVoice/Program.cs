using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SafeVoice.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

// Authorization policies for different roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireModerator", policy => 
        policy.RequireRole("Moderator", "Garda", "SocialServices", "SuperAdmin"));
    
    options.AddPolicy("RequireGarda", policy => 
        policy.RequireRole("Garda", "SocialServices", "SuperAdmin"));
    
    options.AddPolicy("RequireAuthority", policy => 
        policy.RequireRole("Garda", "SocialServices", "SuperAdmin"));
    
    options.AddPolicy("RequireSuperAdmin", policy => 
        policy.RequireRole("SuperAdmin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Support}/{action=Index}/{id?}");
app.Run();