using System.Text.Json.Serialization;

namespace Drivia.Data.SeedDataFiles;

public class ChapterSeedModel
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonPropertyName("notes")]
    public string Notes { get; set; } = string.Empty;

    [JsonPropertyName("order")]
    public int Order { get; set; }
}