using Entities;

namespace Services.Authentication
{
    public interface IAuthenticationService
    {
       public Task<UsersEntity> Login();

    }
}
