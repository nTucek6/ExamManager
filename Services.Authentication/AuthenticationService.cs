using DatabaseContext;
using Entities;
using ExamManager.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ExamManagerContext database;

        private readonly JwtConfiguration jwtConfig;

        public AuthenticationService(ExamManagerContext databaseContext, IOptionsMonitor<JwtConfiguration> jwtConfiguration) { 
            database = databaseContext;
            jwtConfig = jwtConfiguration.CurrentValue;
        }

        public async Task<string> Register(RegisterUserDTO user)
        {
            if ((await database.Users.Where(u => u.Email == user.Email).SingleOrDefaultAsync()) != null)
                throw new Exception("Email taken!");

            string passwordHash = GenerateHash(user.Password);

            await database.Users.AddAsync(
            new UsersEntity
            {
                Email = user.Email,
                PasswordHash = passwordHash,
            });

            await database.SaveChangesAsync();

            string token = await Login(
                new UserLoginDTO
                {
                    Email = user.Email,
                    Password = user.Password
                });
            return token;
        }

        public async Task<string> Login(UserLoginDTO user)
        {
            var userDb = await database.Users.Where(s=> s.Email == user.Email).SingleOrDefaultAsync();

            if(userDb == null)
                throw new Exception("User not found!");
            
            string passwordHash = GenerateHash(user.Password);

            if (userDb.PasswordHash != passwordHash)
                throw new Exception("Invalid password");

            var token = GenerateToken(new GenerateToken
            {
                Id = userDb.Id,
                Email = userDb.Email
            });
            return token;
        }

        private string GenerateToken(GenerateToken user)
        {
            // generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email)
                }),
                //Expires = DateTime.UtcNow.AddDays(jwtConfig.AccessTokenExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public string GenerateHash(string value)
        {
            string valueHash = "";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(value));
                StringBuilder builder = new();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                valueHash = builder.ToString();
            }
            return valueHash;
        }
    }
}
