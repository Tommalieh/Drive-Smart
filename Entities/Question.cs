namespace Drivia.Entities;

public class Question
{
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;
    public bool CorrectAnswer { get; set; } // True/False for now
    public string? Explanation { get; set; } // Can be left empty/null for now
    public Guid ChapterId { get; set; }
    public Chapter Chapter { get; set; } = null!; 
    public string? ChapterTitle { get; set; }  // For display or seeding (optional)
    public string? GroupTitle { get; set; }  // Sub-category or grouping
    public string? ImageUrl { get; set; } // URL or local path
    public int? Difficulty { get; set; } // 1-5 scale or similar, for adaptivity
    public string? Tags { get; set; } // Optional, comma-separated for future use
    public DateTime CreatedAt { get; set; }
}