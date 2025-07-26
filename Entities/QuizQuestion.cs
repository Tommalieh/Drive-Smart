namespace Drivia.Entities;

public class QuizQuestion
{
    public Guid Id { get; set; }
    public Guid QuizSessionId { get; set; }
    public QuizSession QuizSession { get; set; } = null!;
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public bool? UserAnswer { get; set; } // nullable until answered
    public bool? IsCorrect { get; set; } // nullable until answered
}