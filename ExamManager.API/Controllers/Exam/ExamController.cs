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

        [HttpPost]
        public async Task<IActionResult> RegisterStudentExam(RegisterExamDTO studentRegisterDTO)
        {
            await _examService.RegisterStudentExam(studentRegisterDTO);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteStudentExam(int ExamId, int StudentId)
        {
            await _examService.DeleteStudentExam(ExamId, StudentId);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetExamsForRegister(int StudentId)
        {
            var exams = await _examService.GetExamsForRegister(StudentId);
            return Ok(exams);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStudentExams(int StudentId, string? Search)
        {
            var exams = await _examService.GetAllStudentExams(StudentId, Search);
            return Ok(exams);
        }

    }
}
