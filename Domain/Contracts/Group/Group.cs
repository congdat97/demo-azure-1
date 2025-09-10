using Domain.Base;
using Domain.Contracts.GroupPermission;
using Domain.Contracts.UserGroup;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Contracts.Group
{
    public class GroupEntity : Entity
    {
        [Required]
        [MaxLength(50)]
        [Column("group_name")]
        public string GroupName { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }


        public ICollection<UserGroupEntity> UserGroups { get; set; } = new List<UserGroupEntity>();
        public ICollection<GroupPermissionEntity> GroupPermissions { get; set; } = new List<GroupPermissionEntity>();
    }
}
