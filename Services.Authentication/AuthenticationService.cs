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

        public async Task<UsersEntity> Login(UserLoginDTO user)
        {
            var userDb = await database.Users.Where(s=> s.Email == user.Email).SingleOrDefaultAsync();
            return userDb;
        }

    }
}
