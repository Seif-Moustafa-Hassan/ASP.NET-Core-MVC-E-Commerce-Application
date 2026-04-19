using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using WebApplication1.Models;
using ProjectData.Models;

namespace ProjectData.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // =========================
        // DBSets
        // =========================
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        // =========================
        // MODEL CONFIGURATION
        // =========================
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // =========================
            // PRODUCT CONFIG
            // =========================
            builder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");
            });

            // =========================
            // ROLE PERMISSION (COMPOSITE KEY)
            // =========================
            builder.Entity<RolePermission>()
                .HasKey(x => new { x.RoleId, x.PermissionId });

            // =========================
            // MENU HIERARCHY (SELF REFERENCE)
            // =========================
            builder.Entity<MenuItem>()
                .HasOne(m => m.Parent)
                .WithMany(m => m.Children)
                .HasForeignKey(m => m.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // 🔥 MENU ↔ PERMISSION LINK (IMPORTANT)
            // =========================
            builder.Entity<MenuItem>()
                .HasOne(m => m.Permission)
                .WithMany() // one permission can be used by many menu items
                .HasForeignKey(m => m.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // PERMISSION UNIQUE CONSTRAINT
            // =========================
            builder.Entity<Permission>()
                .HasIndex(p => p.Name)
                .IsUnique();
        }
    }
}