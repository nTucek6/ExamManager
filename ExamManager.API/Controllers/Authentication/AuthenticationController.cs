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

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            return Ok("Hello there");
        }

        [HttpPost]
        public async Task<IActionResult> Login()
        {
            var result = await _authenticationService.Login();
            return Ok(result);
        }

    }
}
