using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectData.Data;
using ProjectData.Models;
using ProjectServices.Services.Interfaces;

namespace ProjectServices.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ================= ROLES =================
        public List<IdentityRole> GetRoles() => _roleManager.Roles.ToList();

        public async Task<IdentityRole?> GetRoleByIdAsync(string id)
            => await _roleManager.FindByIdAsync(id);

        public async Task<(bool, List<string>)> CreateRoleAsync(string roleName)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            await _roleManager.DeleteAsync(role);
            return true;
        }

        public async Task<List<Permission>> GetAllPermissionsAsync()
            => await _context.Permissions.ToListAsync();

        public async Task<List<int>> GetRolePermissionsAsync(string roleId)
            => await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

        public async Task UpdateRolePermissionsAsync(string roleId, List<int>? selectedPermissions)
        {
            var existing = _context.RolePermissions.Where(x => x.RoleId == roleId);
            _context.RolePermissions.RemoveRange(existing);

            if (selectedPermissions != null)
            {
                var newPermissions = selectedPermissions.Select(p => new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = p
                });

                await _context.RolePermissions.AddRangeAsync(newPermissions);
            }

            await _context.SaveChangesAsync();
        }

        // ================= PERMISSIONS =================
        public List<Permission> GetPermissions() => _context.Permissions.ToList();

        public Permission? GetPermissionById(int id)
            => _context.Permissions.Find(id);

        public async Task CreatePermissionAsync(Permission permission)
        {
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePermissionAsync(Permission permission)
        {
            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletePermissionAsync(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return false;

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
            return true;
        }

        // ================= USERS =================
        public List<ApplicationUser> GetUsers() => _userManager.Users.ToList();

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
            => await _userManager.FindByIdAsync(userId);

        public async Task<List<string>> GetUserRolesAsync(ApplicationUser user)
            => (await _userManager.GetRolesAsync(user)).ToList();

        public async Task UpdateUserRolesAsync(ApplicationUser user, List<string>? selectedRoles)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (selectedRoles != null)
                await _userManager.AddToRolesAsync(user, selectedRoles);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            await _userManager.DeleteAsync(user);
            return true;
        }

        // ================= MENU =================
        public List<MenuItem> GetParentMenuItems()
            => _context.MenuItems.Where(x => x.ParentId == null).ToList();

        public List<Permission> GetMenuPermissions()
            => _context.Permissions.ToList();

        public async Task<(bool, string)> CreateMenuItemAsync(MenuItem model)
        {
            if (model.ParentId == null)
            {
                model.Controller = null;
                model.Action = null;
                model.PermissionId = null;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.Controller) ||
                    string.IsNullOrWhiteSpace(model.Action) ||
                    model.PermissionId == null)
                {
                    return (false, "Child items must have Controller, Action, and Permission.");
                }
            }

            _context.MenuItems.Add(model);
            await _context.SaveChangesAsync();

            return (true, "");
        }
    }
}