using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Authorization;
using WebApplication1.Data;
using ProjectData.Models;

var builder = WebApplication.CreateBuilder(args);

// =========================
// 🔧 SERVICES
// =========================

// MVC
builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// =========================
// 🔐 IDENTITY
// =========================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// =========================
// 🍪 COOKIE CONFIGURATION
// =========================
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/Login";

    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

// =========================
// 🔐 AUTHORIZATION (PERMISSION SYSTEM)
// =========================

// Dynamic policy provider (IMPORTANT)
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

// Permission handler (DB check)
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

// Base authorization system
builder.Services.AddAuthorization();

// =========================
// 🚀 BUILD APP
// =========================
var app = builder.Build();

// =========================
// 🌐 PIPELINE
// =========================
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

// =========================
// 🧭 ROUTING
// =========================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"
);

app.Run();