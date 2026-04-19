using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Authorization
{
    public class PermissionAttribute : AuthorizeAttribute
    {
        public PermissionAttribute(string permission)
        {
            Policy = permission;
        }
    }
}