using Domain.Base;
using Domain.Contracts.GroupPermission;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Contracts.Permission
{
    public class PermissionEntity : Entity
    {
        [Required]
        [MaxLength(100)]
        [Column("permission_name")]
        public string PermissionName { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }
        public ICollection<GroupPermissionEntity> GroupPermissions { get; set; } = new List<GroupPermissionEntity>();
    }
}
