namespace Drivia.Quizzes;

public class StartQuizResponse
{
    public Guid QuizSessionId { get; set; }
    public List<QuizQuestionDto> Questions { get; set; } = [];
}