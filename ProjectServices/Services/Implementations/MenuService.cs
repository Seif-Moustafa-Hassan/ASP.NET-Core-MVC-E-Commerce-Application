using Microsoft.EntityFrameworkCore;
using ProjectData.Data;
using ProjectServices.Services.Interfaces;

namespace ProjectServices.Services
{
    public class MenuService : IMenuService
    {
        private readonly ApplicationDbContext _context;

        public MenuService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetMenuAsync(string userId)
        {
            // 1. Get user roles
            var roleIds = await _context.UserRoles
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .ToListAsync();

            // 2. Get permissions
            var userPermissions = await _context.RolePermissions
                .Where(x => roleIds.Contains(x.RoleId))
                .Select(x => x.PermissionId)
                .ToListAsync();

            // 3. Load menu
            var menuItems = await _context.MenuItems
                .Include(x => x.Children)
                .Where(x => x.ParentId == null)
                .ToListAsync();

            // 4. Build menu
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

                    // Hide parent if no children
                    if (!children.Any())
                        return null;

                    // Check parent permission
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
                .Where(x => x != null)
                .ToList();

            return result!;
        }
    }
}