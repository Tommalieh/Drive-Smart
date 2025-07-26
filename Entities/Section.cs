namespace DriveSmart.Domain.Entities;

public class Section
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ChapterId { get; set; }
    public Chapter Chapter { get; set; } = null!;

    public string Title { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }  // Optional image

    public int Order { get; set; }  // For sorting sections
}