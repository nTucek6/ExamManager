using Entities;

namespace Services.AdminActions
{
    public interface IAdminActionsService
    {
        public Task<List<SelectUserRoleDTO>> GetUserRoles();
        public Task<List<UsersEntity>> GetUsers();
        public Task ChangeUserRole(UpdateUserDTO userDTO);

    }
}
