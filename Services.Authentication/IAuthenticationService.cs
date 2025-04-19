using Entities;

namespace Services.Authentication
{
    public interface IAuthenticationService
    {
       public Task<string> Login(UserLoginDTO user);
       public Task<string> Register(RegisterUserDTO user);
       public Task ChangePassword(int userId, ChangePasswordDTO model);
       public Task<bool> SendPasswordRestartEmail(string Email);
        public Task RestartPassword(int userId, RestartPasswordDTO changePassword);

    }
}
