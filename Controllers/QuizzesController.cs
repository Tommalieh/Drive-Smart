using System.Security.Claims;
using DriveSmart.Application.Services;
using DriveSmart.Shared.Quizzes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriveSmart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : AppControllerBase
{
    private readonly QuizService _quizService;

    public QuizzesController(QuizService quizService)
    {
        _quizService = quizService;
    }

    [Authorize]
    [HttpPost("start")]
    public ActionResult<StartQuizResponse> StartQuiz([FromBody] StartQuizRequest request)
    {
        var userId = GetUserId();

        // Ignore userId in request; override with the one from token:
        request.UserId = userId;

        var response = _quizService.StartQuiz(request);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("submit")]
    public IActionResult SubmitQuiz([FromBody] SubmitQuizRequest request)
    {
        // Optionally, you could validate ownership of the quiz session using GetUserId() here.
        _quizService.SubmitQuiz(request);
        return Ok();
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<QuizSessionDto> GetQuizById(Guid id)
    {
        // Optionally, you could validate quiz session ownership using GetUserId() here.
        var quiz = _quizService.GetQuizSession(id);
        if (quiz == null)
            return NotFound();
        return Ok(quiz);
    }
}