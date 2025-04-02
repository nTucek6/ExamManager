using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> UpdateExamPeriod(ProfessorExamsDTO exam)
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


    }
}
