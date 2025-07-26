using Drivia.Quizzes;
using Drivia.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Drivia.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController(QuizService quizService) : AppControllerBase
{
    [Authorize]
    [HttpPost("start")]
    public ActionResult<StartQuizResponse> StartQuiz([FromBody] StartQuizRequest request)
    {
        var userId = GetUserId();

        // Ignore userId in request; override with the one from token:
        request.UserId = userId;

        var response = quizService.StartQuiz(request);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("submit")]
    public IActionResult SubmitQuiz([FromBody] SubmitQuizRequest request)
    {
        // Optionally, you could validate ownership of the quiz session using GetUserId() here.
        quizService.SubmitQuiz(request);
        return Ok();
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<QuizSessionDto> GetQuizById(Guid id)
    {
        // Optionally, you could validate quiz session ownership using GetUserId() here.
        var quiz = quizService.GetQuizSession(id);
        if (quiz == null)
            return NotFound();
        return Ok(quiz);
    }
}