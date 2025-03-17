using DatabaseContext;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ExamManagerContext database;

        public AuthenticationService(ExamManagerContext databaseContext) { 
            database = databaseContext;
        }

        public async Task<UsersEntity> Login()
        {
            var user = await database.Users.Where(s=> s.Email == "randomGuy@mail.com").SingleOrDefaultAsync();
            return user;
        }

    }
}
