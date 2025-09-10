using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Group
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; } = null!;
        public string? Description { get; set; }
        public List<string> Permissions { get; set; } = new();
    }
}
