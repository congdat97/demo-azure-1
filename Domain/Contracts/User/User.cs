using Domain.Base;
using Domain.Contracts.UserGroup;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Contracts.User
{
    [Table("users")]
    public class UserEntity : Entity
    {
        [Required]
        [MaxLength(50)]
        [Column("username")]
        public string Username { get; set; } = null!;

        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = null!;

        [MaxLength(100)]
        [Column("full_name")]
        public string? FullName { get; set; }

        [MaxLength(100)]
        [Column("email")]
        public string? Email { get; set; }

        public ICollection<UserGroupEntity> UserGroups { get; set; } = new List<UserGroupEntity>();
    }                                                                                       
}
