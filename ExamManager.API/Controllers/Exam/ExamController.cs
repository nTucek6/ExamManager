using Microsoft.AspNetCore.Mvc;
using Services.Exam;

namespace ExamManager.API.Controllers.Exam
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ExamController : Controller
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService) {
            _examService = examService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentExams(int StudentId)
        {
            var studentExams = await _examService.GetStudentExams(StudentId);
            return Ok(studentExams);
        }
    }
}
