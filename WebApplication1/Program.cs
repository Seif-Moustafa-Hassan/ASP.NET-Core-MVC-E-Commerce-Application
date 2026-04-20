using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
//using WebApplication1.Data;
using ProjectData.Data;
using ProjectData.Models;
using ProjectServices.Services;
using ProjectServices.Services.Implementations;
using ProjectServices.Services.Interfaces;
using WebApplication1.Authorization;

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

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMenuService, MenuService>();

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