using Entities.Enum;

namespace Services.AdminActions
{
    public class SelectUserRoleDTO
    {
        public UserRoleEnum value {  get; set; }
        public string label { get; set; }
    }
}
