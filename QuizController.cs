using Microsoft.AspNetCore.Mvc;

namespace Atika.API
{
    [ApiController]
    [Route("api/quiz")]
    public class QuizController : ControllerBase
    {
        private readonly QuizService _quizService;

        public QuizController(QuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet("question")]
        public IActionResult GetQuestion()
        {
            var response = _quizService.GetQuestion();
            return Ok(response);
        }

        [HttpPost("answer")]
        public IActionResult SubmitAnswer([FromBody] AnswerRequest request)
        {
            var response = _quizService.SubmitAnswer(request);
            return Ok(response);
        }

        [HttpGet("scoreboard")]
        public IActionResult GetScoreboards()
        {
            var response = _quizService.GetScoreboards();
            return Ok(response);
        }
    }
}
