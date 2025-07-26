using System.Text.Json.Serialization;

namespace Drivia.Data.SeedDataFiles;

public class SectionSeedModel
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
        
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
        
    [JsonPropertyName("imageUrl")]
    public string? ImageUrl { get; set; }
        
    [JsonPropertyName("order")]
    public int Order { get; set; }
}