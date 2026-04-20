using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Authorization;
using ProjectServices.Services.Interfaces;
using ProjectData.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // =========================
        // DASHBOARD
        // =========================
        public IActionResult Index()
        {
            return View();
        }

        // ================= ROLE MANAGEMENT =================

        [Permission("View Roles")]
        public IActionResult Roles()
        {
            return View(_adminService.GetRoles());
        }

        [Permission("Create Role")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Create Role")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                TempData["Error"] = "Role name is required";
                return View();
            }

            var (success, errors) = await _adminService.CreateRoleAsync(roleName);

            if (success)
            {
                TempData["Success"] = "Role created successfully";
                return RedirectToAction(nameof(Roles));
            }

            TempData["Error"] = string.Join(",", errors);
            return View();
        }

        [Permission("Edit Role")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _adminService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();

            var permissions = await _adminService.GetAllPermissionsAsync();
            var rolePermissions = await _adminService.GetRolePermissionsAsync(id);

            var model = permissions.Select(p => new
            {
                p.Id,
                p.Name,
                IsSelected = rolePermissions.Contains(p.Id)
            }).ToList();

            ViewBag.RoleId = id;
            ViewBag.RoleName = role.Name;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Edit Role")]
        public async Task<IActionResult> EditRole(string roleId, List<int> selectedPermissions)
        {
            await _adminService.UpdateRolePermissionsAsync(roleId, selectedPermissions);

            TempData["Success"] = "Permissions updated successfully";
            return RedirectToAction(nameof(Roles));
        }

        [Permission("Delete Role")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _adminService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();

            return View(role);
        }

        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        [Permission("Delete Role")]
        public async Task<IActionResult> DeleteRoleConfirmed(string id)
        {
            var success = await _adminService.DeleteRoleAsync(id);

            if (!success)
            {
                TempData["Error"] = "Role not found";
                return RedirectToAction(nameof(Roles));
            }

            TempData["Success"] = "Role deleted successfully";
            return RedirectToAction(nameof(Roles));
        }

        // ================= PERMISSION MANAGEMENT =================

        [Permission("View Permissions")]
        public IActionResult Permissions()
        {
            return View(_adminService.GetPermissions());
        }

        [Permission("Create Permission")]
        public IActionResult CreatePermission()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Create Permission")]
        public async Task<IActionResult> CreatePermission(Permission permission)
        {
            if (!ModelState.IsValid)
                return View(permission);

            await _adminService.CreatePermissionAsync(permission);

            TempData["Success"] = "Permission created successfully";
            return RedirectToAction(nameof(Permissions));
        }

        [Permission("Edit Permission")]
        public IActionResult EditPermission(int id)
        {
            var permission = _adminService.GetPermissionById(id);
            if (permission == null) return NotFound();

            return View(permission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Edit Permission")]
        public async Task<IActionResult> EditPermission(Permission permission)
        {
            if (!ModelState.IsValid)
                return View(permission);

            await _adminService.UpdatePermissionAsync(permission);

            TempData["Success"] = "Permission updated successfully";
            return RedirectToAction(nameof(Permissions));
        }

        [Permission("Delete Permission")]
        public IActionResult DeletePermission(int id)
        {
            var permission = _adminService.GetPermissionById(id);
            if (permission == null) return NotFound();

            return View(permission);
        }

        [HttpPost, ActionName("DeletePermission")]
        [ValidateAntiForgeryToken]
        [Permission("Delete Permission")]
        public async Task<IActionResult> DeletePermissionConfirmed(int id)
        {
            var success = await _adminService.DeletePermissionAsync(id);

            if (!success)
            {
                TempData["Error"] = "Permission not found";
                return RedirectToAction(nameof(Permissions));
            }

            TempData["Success"] = "Permission deleted successfully";
            return RedirectToAction(nameof(Permissions));
        }

        // ================= USER MANAGEMENT =================

        [Permission("View Users")]
        public IActionResult Users()
        {
            return View(_adminService.GetUsers());
        }

        [Permission("Edit User")]
        public async Task<IActionResult> EditUserRoles(string userId)
        {
            var user = await _adminService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = _adminService.GetRoles();
            var userRoles = await _adminService.GetUserRolesAsync(user);

            var model = roles.Select(r => new
            {
                r.Name,
                IsSelected = userRoles.Contains(r.Name)
            }).ToList();

            ViewBag.UserId = userId;
            ViewBag.UserName = user.UserName;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Edit User")]
        public async Task<IActionResult> EditUserRoles(string userId, List<string> selectedRoles)
        {
            var user = await _adminService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            await _adminService.UpdateUserRolesAsync(user, selectedRoles);

            TempData["Success"] = "User roles updated successfully";
            return RedirectToAction(nameof(Users));
        }

        [Permission("Delete User")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _adminService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        [Permission("Delete User")]
        public async Task<IActionResult> DeleteUserConfirmed(string userId)
        {
            var success = await _adminService.DeleteUserAsync(userId);

            if (!success)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction(nameof(Users));
            }

            TempData["Success"] = "User deleted successfully";
            return RedirectToAction(nameof(Users));
        }

        // ================= MENU =================

        [Permission("Create Menu Item")]
        public IActionResult CreateMenuItem()
        {
            ViewBag.Parents = _adminService.GetParentMenuItems();
            ViewBag.Permissions = _adminService.GetMenuPermissions();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Create Permission")]
        public async Task<IActionResult> CreateMenuItem(MenuItem model)
        {
            var (success, error) = await _adminService.CreateMenuItemAsync(model);

            if (!success)
            {
                TempData["Error"] = error;
                return RedirectToAction(nameof(CreateMenuItem));
            }

            TempData["Success"] = "Menu item created successfully";
            return RedirectToAction(nameof(Permissions));
        }
    }
}