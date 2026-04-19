using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
//using WebApplication1.Data;
using ProjectData.Data;

namespace WebApplication1.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ApplicationDbContext _context;

        public PermissionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return;

            var hasPermission = await (
                from rp in _context.RolePermissions
                join p in _context.Permissions on rp.PermissionId equals p.Id
                join ur in _context.UserRoles on rp.RoleId equals ur.RoleId
                where ur.UserId == userId && p.Name == requirement.Permission
                select rp
            ).AnyAsync();

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}