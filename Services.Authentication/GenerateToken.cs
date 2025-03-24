using Entities.Enum;

namespace Services.Authentication
{
    public class GenerateToken
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public UserRoleEnum Role { get; set; }
    }
}
