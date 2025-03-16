using DatabaseContext;

namespace Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ExamManagerContext database;

        public AuthenticationService(ExamManagerContext databaseContext) { 
            database = databaseContext;
        }

        public async Task<string> Login()
        {
            return "Login";
        }

    }
}
