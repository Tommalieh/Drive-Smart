namespace Drivia.Quizzes;

public class QuizSessionDto
{
    public Guid QuizSessionId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string? Type { get; set; }
    public List<QuizQuestionDto> Questions { get; set; } = [];
    public int? Score { get; set; }
}