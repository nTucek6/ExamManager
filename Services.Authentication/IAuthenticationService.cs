namespace Services.Authentication
{
    public interface IAuthenticationService
    {
       public Task<string> Login();

    }
}
