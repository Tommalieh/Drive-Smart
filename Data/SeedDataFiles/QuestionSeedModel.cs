using System.Text.Json.Serialization;

namespace DriveSmart.Persistence.Data.SeedDataFiles;

public class QuestionSeedModel
{
    [JsonPropertyName("question")]
    public string Question { get; set; }

    [JsonPropertyName("group")]
    public string Group { get; set; }

    [JsonPropertyName("correctAnswer")]
    public bool CorrectAnswer { get; set; }

    [JsonPropertyName("imageUrl")]
    public string? ImageUrl { get; set; } // Or string? if optional

    [JsonPropertyName("difficulty")]
    public int? Difficulty { get; set; } // Not present in your file, but for future

    [JsonPropertyName("tags")]
    public string? Tags { get; set; }    // Not present in your file, but for future
}