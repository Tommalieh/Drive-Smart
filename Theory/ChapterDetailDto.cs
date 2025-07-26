namespace Drivia.Theory;

public class ChapterDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    public List<SectionDto> Sections { get; set; } = new();

    public Guid? LastViewedSectionId { get; set; }
    public int? LastViewedSectionOrder { get; set; }
}