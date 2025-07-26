namespace Drivia.Quizzes;

public class StartQuizRequest
{
    public string? Type { get; set; } // "random", "chapter", "adaptive"
    public List<Guid>? ChapterIds { get; set; }
    public int NumberOfQuestions { get; set; }
    public Guid UserId { get; set; }
}