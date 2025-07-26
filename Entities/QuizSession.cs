namespace DriveSmart.Domain.Entities;

public class QuizSession
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string Type { get; set; } // "random", "chapter", "adaptive"
    public ICollection<QuizQuestion> QuizQuestions { get; set; }
}