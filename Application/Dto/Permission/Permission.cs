using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Permission
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string PermissionName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
