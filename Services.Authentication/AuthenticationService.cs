using DatabaseContext;
using Entities;
using Entities.Enum;
using ExamManager.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.EmailSender;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ExamManagerContext database;

        private readonly IEmailSenderService emailSenderService;

        private readonly JwtConfiguration jwtConfig;

        public AuthenticationService(ExamManagerContext databaseContext, IOptionsMonitor<JwtConfiguration> jwtConfiguration, IEmailSenderService emailSenderService) { 
            database = databaseContext;
            jwtConfig = jwtConfiguration.CurrentValue;
            this.emailSenderService = emailSenderService;
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
                Name = user.Name,
                Surname = user.Surname,
                RoleId = UserRoleEnum.Student,
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
                Email = userDb.Email,
                Role = userDb.RoleId,
            }, false);
            return token;
        }

        private string GenerateToken(GenerateToken user, bool useExpires)
        {
            // generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("Role", user.Role.ToString()),
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            if(useExpires)
            {
                tokenDescriptor.Expires = DateTime.UtcNow.AddHours(jwtConfig.AccessTokenExpiration);
            }
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

        public async Task ChangePassword(int userId, ChangePasswordDTO model)
        {
            var user = await database.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            string oldPasswordHash = GenerateHash(model.OldPassword);
            if (user.PasswordHash != oldPasswordHash)
            {
                throw new Exception("Old password is incorrect");
            }

            user.PasswordHash = GenerateHash(model.NewPassword);
            await database.SaveChangesAsync();
        }

        public async Task<bool> SendPasswordRestartEmail(string Email)
        {
            var user = await database.Users.Where(q => q.Email == Email).FirstOrDefaultAsync();
            if(user != null)
            {
                var token = GenerateToken(new GenerateToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = user.RoleId,
                }, true);
                string Subject = "Examio: Restart password";
                var resetLink = $"http://localhost:3000/restart-password/{Uri.EscapeDataString(token)}";
                var Message = $@"
                            <html>
                                <body>
                                <p>Kliknite na priloženi link kako biste ponovno postavili lozinku za Examio:</p>
                                <a href='{resetLink}' target='_blank' rel='noopener noreferrer'>Ovdje</a>
                                </body>
                            </html>";
                await emailSenderService.SendEmail(user.Email, Subject, Message);
                return true;
            }
            return false;
        }

        public async Task RestartPassword(int userId, RestartPasswordDTO changePassword)
        {
            var user = await database.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.PasswordHash = GenerateHash(changePassword.Password);
            await database.SaveChangesAsync();
        }

    }
}
