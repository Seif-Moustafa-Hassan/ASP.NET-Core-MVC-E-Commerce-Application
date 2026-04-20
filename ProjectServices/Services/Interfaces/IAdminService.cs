using Microsoft.AspNetCore.Identity;
using ProjectData.Models;

namespace ProjectServices.Services.Interfaces
{
    public interface IAdminService
    {
        // ================= ROLES =================
        List<IdentityRole> GetRoles();
        Task<IdentityRole?> GetRoleByIdAsync(string id);
        Task<(bool Success, List<string> Errors)> CreateRoleAsync(string roleName);
        Task<bool> DeleteRoleAsync(string roleId);

        Task<List<Permission>> GetAllPermissionsAsync();
        Task<List<int>> GetRolePermissionsAsync(string roleId);
        Task UpdateRolePermissionsAsync(string roleId, List<int>? selectedPermissions);

        // ================= PERMISSIONS =================
        List<Permission> GetPermissions();
        Permission? GetPermissionById(int id);
        Task CreatePermissionAsync(Permission permission);
        Task UpdatePermissionAsync(Permission permission);
        Task<bool> DeletePermissionAsync(int id);

        // ================= USERS =================
        List<ApplicationUser> GetUsers();
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<List<string>> GetUserRolesAsync(ApplicationUser user);
        Task UpdateUserRolesAsync(ApplicationUser user, List<string>? selectedRoles);
        Task<bool> DeleteUserAsync(string userId);

        // ================= MENU =================
        List<MenuItem> GetParentMenuItems();
        List<Permission> GetMenuPermissions();
        Task<(bool Success, string Error)> CreateMenuItemAsync(MenuItem model);
    }
}