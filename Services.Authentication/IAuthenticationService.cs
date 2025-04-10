using Entities;

namespace Services.Authentication
{
    public interface IAuthenticationService
    {
       public Task<string> Login(UserLoginDTO user);
       public Task<string> Register(RegisterUserDTO user);
       public Task ChangePassword(int userId, ChangePasswordDTO model);

    }
}
