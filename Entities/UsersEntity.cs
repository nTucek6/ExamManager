using Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class UsersEntity
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public UserRoleEnum RoleId { get; set; }
    }
}
