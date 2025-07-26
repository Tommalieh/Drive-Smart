using System.Text.Json;
using DriveSmart.Domain.Entities;
using DriveSmart.Persistence.Data.SeedDataFiles;

namespace DriveSmart.Persistence.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Chapters.Any())
            {
                Console.WriteLine("Chapters already exist. Skipping seeding.");
                return;
            }

            var basePath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedDataFiles");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // Load all chapter-N.json files (sorted)
            var chapterFiles = Directory.GetFiles(basePath, "chapter-*.json")
                                        .Where(f => !f.Contains("-sections") && !f.Contains("-questions"))
                                        .OrderBy(f => f)
                                        .ToList();

            foreach (var chapterFile in chapterFiles)
            {
                var chapterJson = File.ReadAllText(chapterFile);
                var chapterSeed = JsonSerializer.Deserialize<ChapterSeedModel>(chapterJson, options);
                if (chapterSeed == null)
                {
                    Console.WriteLine($"Failed to parse chapter seed for file: {chapterFile}");
                    continue;
                }

                var chapter = new Chapter
                {
                    Title = chapterSeed.Title,
                    Content = chapterSeed.Content,
                    Summary = chapterSeed.Summary,
                    Notes = chapterSeed.Notes,
                    Order = chapterSeed.Order
                };

                context.Chapters.Add(chapter);
                context.SaveChanges(); // save here to get chapter.Id

                Console.WriteLine($"Seeded chapter: {chapter.Title}");

                // Seed sections if file exists
                var baseFileName = Path.GetFileNameWithoutExtension(chapterFile); // e.g., chapter-1
                var sectionsPath = Path.Combine(basePath, $"{baseFileName}-sections.json");
                if (File.Exists(sectionsPath))
                {
                    var sectionJson = File.ReadAllText(sectionsPath);
                    var sectionSeeds = JsonSerializer.Deserialize<List<SectionSeedModel>>(sectionJson, options);
                    if (sectionSeeds is not null)
                    {
                        var sections = sectionSeeds.Select(s => new Section
                        {
                            ChapterId = chapter.Id,
                            Title = s.Title,
                            Text = s.Text,
                            ImageUrl = s.ImageUrl,
                            Order = s.Order
                        }).ToList();

                        context.Sections.AddRange(sections);
                        context.SaveChanges();
                        Console.WriteLine($"Seeded {sections.Count} sections for chapter: {chapter.Title}");
                    }
                }
                else
                {
                    Console.WriteLine($"No sections file found for {baseFileName}");
                }

                // Seed questions if file exists
                var questionsPath = Path.Combine(basePath, $"{baseFileName}-questions.json");
                if (File.Exists(questionsPath))
                {
                    try
                    {
                        var questionsJson = File.ReadAllText(questionsPath);
                        var questionSeeds = JsonSerializer.Deserialize<List<QuestionSeedModel>>(questionsJson, options);

                        if (questionSeeds == null || !questionSeeds.Any())
                        {
                            Console.WriteLine($"No questions loaded for {baseFileName}. Check JSON format and model mapping!");
                        }
                        else
                        {
                            var questions = questionSeeds.Select(q => new Question
                            {
                                ChapterId = chapter.Id,
                                ChapterTitle = chapter.Title,
                                GroupTitle = q.Group,
                                Text = q.Question,
                                ImageUrl = q.ImageUrl,
                                CorrectAnswer = q.CorrectAnswer,
                                Explanation = null,
                                Difficulty = q.Difficulty,
                                Tags = q.Tags,
                                CreatedAt = DateTime.UtcNow
                            }).ToList();

                            context.Questions.AddRange(questions);
                            context.SaveChanges();

                            Console.WriteLine($"Seeded {questions.Count} questions for chapter: {chapter.Title}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error seeding questions for {baseFileName}: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Questions file not found: {questionsPath}");
                }
            }
        }
    }
}
