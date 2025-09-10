using Domain.Base;
using Domain.Contracts.Group;
using Domain.Contracts.Permission;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Contracts.GroupPermission
{
    public class GroupPermissionEntity : Entity
    {
        [Column("group_id")]
        public int GroupId { get; set; }

        [Column("permission_id")]
        public int PermissionId { get; set; }

        public GroupEntity Group { get; set; } = null!;
        public PermissionEntity Permission { get; set; } = null!;
    }
}
