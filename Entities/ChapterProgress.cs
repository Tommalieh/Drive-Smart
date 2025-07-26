namespace Drivia.Entities;

public class ChapterProgress
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }  // Foreign key

    public Guid ChapterId { get; set; }  // Foreign key

    public bool IsCompleted { get; set; }

    public DateTime LastViewedAt { get; set; } = DateTime.UtcNow;
}