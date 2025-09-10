using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.User
{
    public class UserJoinEntity
    {
        // User
        public int UserId { get; set; }
        public string Username { get; set; } = default!;
        public string Password_Hash { get; set; } = default!;
        public string Full_Name { get; set; } = default!;
        public string Email { get; set; } = default!;

        // Group
        public int? GroupId { get; set; }
        public string? Group_Name { get; set; }
        public string? Group_Description { get; set; }

        // Permission
        public int? PermissionId { get; set; }
        public string? Permission_Name { get; set; }
        public string? Permission_Description { get; set; }
    }
}
