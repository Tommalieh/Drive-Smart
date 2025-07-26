namespace DriveSmart.Shared.Theory;

public class SectionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int Order { get; set; }
    
    public bool IsCompleted { get; set; } 
}