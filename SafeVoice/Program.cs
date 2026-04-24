using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SafeVoice.Data;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
// Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Cookie Authentication
var authBuilder = builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.SlidingExpiration = true;
        })
        .AddGitHub(options => {
            options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"];
            options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"];
            options.CallbackPath = "/signin-github";
            options.Scope.Add("user:email");
        })
        .AddGoogle(options => {
            options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            options.CallbackPath = "/signin-google";
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
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

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