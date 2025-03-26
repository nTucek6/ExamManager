using Microsoft.AspNetCore.Mvc;
using Services.Authentication;

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

    }
}
