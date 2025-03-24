using Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class UsersEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required UserRoleEnum RoleId { get; set; }
    }
}
