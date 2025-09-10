using Domain.Contracts.Group;
using Domain.Contracts.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Domain.Contracts.UserGroup
{

    [Table("user_groups")]
    public class UserGroupEntity 
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("group_id")]
        public int GroupId { get; set; }


        public UserEntity User { get; set; } = null!;
        public GroupEntity Group { get; set; } = null!;
    }

}
