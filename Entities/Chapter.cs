namespace Drivia.Entities;

public class Chapter
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public int Order { get; set; }  // For ordering chapters in sequence
    
    public ICollection<Section> Sections { get; set; } = new List<Section>();

}