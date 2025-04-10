using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Services.ProfessorExam;

namespace ExamManager.API.Controllers.ProfessorExam
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProfessorExamController : Controller
    {
        private readonly IProfessorExamService _professorExamService;

        public ProfessorExamController(IProfessorExamService professorExamService)
        {
            _professorExamService = professorExamService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfessorExams(int ProfessorId)
        {
            var studentExams = await _professorExamService.GetProfessorExams(ProfessorId);
            return Ok(studentExams);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExamPeriod(NewExamDTO newExamDTO)
        {
            await _professorExamService.CreateExamPeriod(newExamDTO);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateExamPeriod(NewExamDTO exam)
        {
            await _professorExamService.UpdateExamPeriod(exam);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExamPeriod(int examId)
        {
            await _professorExamService.DeleteExamPeriod(examId);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetExamPeriod(int ExamId)
        {
            var exam = await _professorExamService.GetExamPeriod(ExamId);
            return Ok(exam);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfessorSubjects(int ProfessorId)
        {
            var subjects = await _professorExamService.GetProfessorSubjects(ProfessorId);
            return Ok(subjects);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfessorExamStudents(int ExamId)
        {
            var students = await _professorExamService.GetProfessorExamStudents(ExamId);
            return Ok(students);
        }


    }
}
