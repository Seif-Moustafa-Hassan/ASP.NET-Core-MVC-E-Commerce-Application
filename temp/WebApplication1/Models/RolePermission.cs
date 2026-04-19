using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace WebApplication1.Models
{
    public class RolePermission
    {
        [ForeignKey(nameof(Role))]
        public string RoleId { get; set; } = null!;

        [ForeignKey(nameof(Permission))]
        public int PermissionId { get; set; }

        public IdentityRole Role { get; set; } = null!;
        public Permission Permission { get; set; } = null!;
    }
}
