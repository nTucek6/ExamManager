using Microsoft.AspNetCore.Mvc;
using Services.AdminActions;
using Services.EmailSender;

namespace ExamManager.API.Controllers.AdminActions
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminActionsController : Controller
    {
        private readonly IAdminActionsService _adminActionsService;

        private readonly IEmailSenderService emailSenderService;

        public AdminActionsController(IAdminActionsService adminActionsService, IEmailSenderService emailSenderService)
        {
            _adminActionsService = adminActionsService;
            this.emailSenderService = emailSenderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserRoles()
        {
            var roles = await _adminActionsService.GetUserRoles();
            return Ok(roles);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _adminActionsService.GetUsers();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(UpdateUserDTO userDTO)
        {
            await _adminActionsService.ChangeUserRole(userDTO);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> SendEmailTester()
        {
            await emailSenderService.SendEmailToStudents();
            return Ok();
        }


    }
}
