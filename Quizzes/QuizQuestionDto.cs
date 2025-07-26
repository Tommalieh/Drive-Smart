namespace Drivia.Quizzes;

public class QuizQuestionDto
{
    public Guid QuestionId { get; set; }
    public string? Text { get; set; }
    public string? GroupTitle { get; set; }
    public string? ImageUrl { get; set; }
    public bool? UserAnswer { get; set; }
}