using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
//using WebApplication1.Data;
using ProjectData.Data;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/menu")]
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMenu()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 1. Get user roles
            var roleIds = await _context.UserRoles
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .ToListAsync();

            // 2. Get permissions for those roles
            var userPermissions = await _context.RolePermissions
                .Where(x => roleIds.Contains(x.RoleId))
                .Select(x => x.PermissionId)
                .ToListAsync();

            // 3. Load menu
            var menuItems = await _context.MenuItems
                .Include(x => x.Children)
                .Where(x => x.ParentId == null)
                .ToListAsync();

            // 4. Build response
            var result = menuItems
                .Select(parent =>
                {
                    var children = parent.Children
                        .Where(c =>
                            c.PermissionId == null ||
                            (c.PermissionId.HasValue && userPermissions.Contains(c.PermissionId.Value))
                        )
                        .Select(c => new
                        {
                            c.Title,
                            c.Controller,
                            c.Action
                        })
                        .ToList();

                    // 🔥 KEY RULE: hide parent if no children
                    if (!children.Any())
                        return null;

                    // also filter parent itself
                    if (parent.PermissionId != null &&
                        !userPermissions.Contains(parent.PermissionId.Value))
                        return null;

                    return new
                    {
                        parent.Title,
                        parent.Controller,
                        parent.Action,
                        Children = children
                    };
                })
                .Where(x => x != null);

            return Json(result);
        }
    }
}