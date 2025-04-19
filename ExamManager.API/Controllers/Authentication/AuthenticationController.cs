using Microsoft.AspNetCore.Mvc;
using Services.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace ExamManager.API.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService) { 
        _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO user)
        {
            var result = await _authenticationService.Login(user);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDTO user)
        {
            var result = await _authenticationService.Register(user);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            try
            {
                // Dohvati token iz Authorization headera
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                // Ekstraktiraj korisnički ID iz tokena
                var userId = GetUserIdFromToken(token);

                if (userId == 0)
                {
                    return Unauthorized("Invalid token");
                }

                // Pozovi servis za promjenu lozinke
                await _authenticationService.ChangePassword(userId, model);
                return Ok("Password changed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Metoda za ekstrakciju ID-a iz JWT tokena
        private int GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Pretpostavljamo da je 'Id' claim spremljen u tokenu
            var userIdClaim = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
                return 0;

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            return 0;
        }

        private int ValidateToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "Id");

            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                return 0;
            }

            if (userIdClaim == null)
                return 0;

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 0;
        }


        [HttpPost]
        public async Task<IActionResult> SendPasswordRestartEmail(string Email)
        {
            bool success = await _authenticationService.SendPasswordRestartEmail(Email);
            return Ok(success);
        }

        [HttpPost]
        public async Task<IActionResult> RestartPassword([FromBody] RestartPasswordDTO restartPassword)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                int userId = ValidateToken(token);

                if (userId == 0) {
                    return Unauthorized("Invalid token");
                }

                await _authenticationService.RestartPassword(userId, restartPassword);

                return Ok("Password restarted successfully");

            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }           
        }


    }
}
