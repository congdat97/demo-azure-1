using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public List<string> Groups { get; set; } = new();
    }

    public class UserWithPermissionsDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public List<string> Permissions { get; set; } = new();
    }


    public class UserJoinDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;

        // Quan hệ n-n với Group
        public List<UserGroupDto> UserGroups { get; set; } = new();
    }

    public class UserGroupDto
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public GroupDto Group { get; set; } = default!;
    }

    public class GroupDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; } = default!;
        public string? Description { get; set; }

        // Quan hệ n-n với Permission
        public List<GroupPermissionDto> GroupPermissions { get; set; } = new();
    }

    public class GroupPermissionDto
    {
        public int GroupId { get; set; }
        public int PermissionId { get; set; }

        public GroupDto Group { get; set; } = default!;
        public PermissionDto Permission { get; set; } = default!;
    }

    public class PermissionDto 
    {
        public int Id { get; set; }
        public string PermissionName { get; set; } = default!;
        public string? Description { get; set; }
    }

}
