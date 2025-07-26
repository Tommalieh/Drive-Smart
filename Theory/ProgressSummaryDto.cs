namespace Drivia.Theory;

public class ProgressSummaryDto
{
    public int TotalChapters { get; set; }
    public int CompletedChapters { get; set; }

    public double ReadinessScore { get; set; }

    public List<ChapterProgressDetail> ChaptersYouRock { get; set; } = new();
    public List<ChapterProgressDetail> ChaptersToImprove { get; set; } = new();
}

public class ChapterProgressDetail
{
    public Guid ChapterId { get; set; }
    public string Title { get; set; } = string.Empty;
    public double CompletionRate { get; set; }
}