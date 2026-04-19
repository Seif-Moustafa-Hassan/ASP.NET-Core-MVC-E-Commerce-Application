using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Authorization;
using WebApplication1.Data;
using ProjectData.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // =========================
        // DASHBOARD
        // =========================
        public IActionResult Index()
        {
            return View();
        }

        // =====================================================
        // ================= ROLE MANAGEMENT ===================
        // =====================================================

        [Permission("View Roles")]
        public IActionResult Roles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
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

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (result.Succeeded)
            {
                TempData["Success"] = "Role created successfully";
                return RedirectToAction(nameof(Roles));
            }

            TempData["Error"] = string.Join(",", result.Errors.Select(e => e.Description));
            return View();
        }

        [Permission("Edit Role")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var permissions = await _context.Permissions.ToListAsync();

            var rolePermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == id)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

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

            TempData["Success"] = "Permissions updated successfully";
            return RedirectToAction(nameof(Roles));
        }

        [Permission("Delete Role")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            return View(role);
        }

        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        [Permission("Delete Role")]
        public async Task<IActionResult> DeleteRoleConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                TempData["Error"] = "Role not found";
                return RedirectToAction(nameof(Roles));
            }

            await _roleManager.DeleteAsync(role);

            TempData["Success"] = "Role deleted successfully";
            return RedirectToAction(nameof(Roles));
        }

        // =====================================================
        // ============== PERMISSION MANAGEMENT =================
        // =====================================================

        [Permission("View Permissions")]
        public IActionResult Permissions()
        {
            var permissions = _context.Permissions.ToList();
            return View(permissions);
        }

        [Permission("Create Permission")]
        public IActionResult CreatePermission()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Create Permission")]
        public IActionResult CreatePermission(Permission permission)
        {
            if (!ModelState.IsValid)
                return View(permission);

            _context.Permissions.Add(permission);
            _context.SaveChanges();

            TempData["Success"] = "Permission created successfully";
            return RedirectToAction(nameof(Permissions));
        }

        [Permission("Edit Permission")]
        public IActionResult EditPermission(int id)
        {
            var permission = _context.Permissions.Find(id);
            if (permission == null) return NotFound();

            return View(permission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Edit Permission")]
        public IActionResult EditPermission(Permission permission)
        {
            if (!ModelState.IsValid)
                return View(permission);

            _context.Permissions.Update(permission);
            _context.SaveChanges();

            TempData["Success"] = "Permission updated successfully";
            return RedirectToAction(nameof(Permissions));
        }

        [Permission("Delete Permission")]
        public IActionResult DeletePermission(int id)
        {
            var permission = _context.Permissions.Find(id);
            if (permission == null) return NotFound();

            return View(permission);
        }

        [HttpPost, ActionName("DeletePermission")]
        [ValidateAntiForgeryToken]
        [Permission("Delete Permission")]
        public IActionResult DeletePermissionConfirmed(int id)
        {
            var permission = _context.Permissions.Find(id);

            if (permission == null)
            {
                TempData["Error"] = "Permission not found";
                return RedirectToAction(nameof(Permissions));
            }

            _context.Permissions.Remove(permission);
            _context.SaveChanges();

            TempData["Success"] = "Permission deleted successfully";
            return RedirectToAction(nameof(Permissions));
        }

        // =====================================================
        // ================= USER MANAGEMENT ====================
        // =====================================================

        [Permission("View Users")]
        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [Permission("Edit User")]
        public async Task<IActionResult> EditUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);

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
            var user = await _userManager.FindByIdAsync(userId);

            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (selectedRoles != null)
                await _userManager.AddToRolesAsync(user, selectedRoles);

            TempData["Success"] = "User roles updated successfully";
            return RedirectToAction(nameof(Users));
        }

        [Permission("Delete User")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        [Permission("Delete User")]
        public async Task<IActionResult> DeleteUserConfirmed(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction(nameof(Users));
            }

            await _userManager.DeleteAsync(user);

            TempData["Success"] = "User deleted successfully";
            return RedirectToAction(nameof(Users));
        }

        [Permission("Create Menu Item")]
        public IActionResult CreateMenuItem()
        {
            ViewBag.Parents = _context.MenuItems
                .Where(x => x.ParentId == null)
                .ToList();

            ViewBag.Permissions = _context.Permissions.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Create Permission")]
        public IActionResult CreateMenuItem(MenuItem model)
        {
            // RULE 1: Parent item
            if (model.ParentId == null)
            {
                model.Controller = null;
                model.Action = null;
                model.PermissionId = null;
            }

            // RULE 2: Child item validation
            else
            {
                if (string.IsNullOrWhiteSpace(model.Controller) ||
                    string.IsNullOrWhiteSpace(model.Action) ||
                    model.PermissionId == null)
                {
                    TempData["Error"] = "Child items must have Controller, Action, and Permission.";
                    return RedirectToAction("CreateMenuItem");
                }
            }

            _context.MenuItems.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Menu item created successfully";
            return RedirectToAction("Permissions"); // or any page you prefer
        }



    }
}