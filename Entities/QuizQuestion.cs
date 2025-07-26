namespace DriveSmart.Domain.Entities;

public class QuizQuestion
{
    public Guid Id { get; set; }
    public Guid QuizSessionId { get; set; }
    public QuizSession QuizSession { get; set; }
    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
    public bool? UserAnswer { get; set; } // nullable until answered
    public bool? IsCorrect { get; set; }  // nullable until answered
}