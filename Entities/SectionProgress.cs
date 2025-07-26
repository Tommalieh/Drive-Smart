namespace Drivia.Entities;

public class SectionProgress
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid UserId { get; set; }
    public Guid SectionId { get; set; }

    public bool IsCompleted { get; set; }
    public DateTime LastViewedAt { get; set; } = DateTime.UtcNow;
}