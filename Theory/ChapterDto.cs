namespace DriveSmart.Shared.Theory;

public class ChapterDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsCompleted { get; set; }

    public Guid? LastViewedSectionId { get; set; }
    public int? LastViewedSectionOrder { get; set; }
}