using DriveSmart.Domain.Entities;
using DriveSmart.Persistence.Data;
using DriveSmart.Shared.Theory;
using Microsoft.EntityFrameworkCore;

namespace DriveSmart.Application.Services;

public class TheoryService
{
    private readonly AppDbContext _context;

    public TheoryService(AppDbContext context)
    {
        _context = context;
    }

    public List<ChapterDto> GetChapters(Guid userId)
    {
        var progress = _context.ChapterProgress
            .Where(p => p.UserId == userId)
            .ToDictionary(p => p.ChapterId, p => p.IsCompleted);

        return _context.Chapters
            .OrderBy(c => c.Order)
            .Select(c => new ChapterDto
            {
                Id = c.Id,
                Title = c.Title,
                Summary = c.Summary,
                Order = c.Order,
                IsCompleted = progress.ContainsKey(c.Id) && progress[c.Id]
            })
            .ToList();
    }
    
    public ChapterDetailDto? GetChapterById(Guid userId, Guid chapterId)
    {
        var chapter = _context.Chapters
            .Include(c => c.Sections)
            .FirstOrDefault(c => c.Id == chapterId);

        if (chapter == null) return null;

        var sectionProgress = _context.SectionProgress
            .Where(p => p.UserId == userId)
            .GroupBy(p => p.SectionId)
            .ToDictionary(
                g => g.Key,
                g => g.OrderByDescending(p => p.LastViewedAt).First().IsCompleted
            );

        return new ChapterDetailDto
        {
            Id = chapter.Id,
            Title = chapter.Title,
            Content = chapter.Content,
            Summary = chapter.Summary,
            Notes = chapter.Notes,
            Sections = chapter.Sections
                .OrderBy(s => s.Order)
                .Select(s => new SectionDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Text = s.Text,
                    ImageUrl = s.ImageUrl,
                    Order = s.Order,
                    IsCompleted = sectionProgress.ContainsKey(s.Id) && sectionProgress[s.Id]
                }).ToList()
        };
    }
    
    public void MarkSectionViewed(Guid userId, Guid sectionId, bool isCompleted)
    {
        var progress = _context.SectionProgress
            .FirstOrDefault(p => p.UserId == userId && p.SectionId == sectionId);

        if (progress is not null)
        {
            progress.IsCompleted = isCompleted;
            progress.LastViewedAt = DateTime.UtcNow;
        }
        else
        {
            _context.SectionProgress.Add(new SectionProgress
            {
                UserId = userId,
                SectionId = sectionId,
                IsCompleted = isCompleted,
                LastViewedAt = DateTime.UtcNow
            });
        }

        _context.SaveChanges();

        // ✅ Get the chapter of the section
        var section = _context.Sections.Find(sectionId);
        if (section == null) return;

        var chapterId = section.ChapterId;

        // ✅ Get all section IDs in the chapter
        var allSectionIds = _context.Sections
            .Where(s => s.ChapterId == chapterId)
            .Select(s => s.Id)
            .ToList();

        // ✅ Get all completed section IDs by this user in the chapter
        var completedSectionIds = _context.SectionProgress
            .Where(p => p.UserId == userId && p.IsCompleted && allSectionIds.Contains(p.SectionId))
            .Select(p => p.SectionId)
            .ToList();

        // ✅ If all are completed, mark the chapter as completed
        if (completedSectionIds.Count == allSectionIds.Count)
        {
            MarkChapterCompleted(userId, chapterId, true);
        }
    }

    
    public void MarkChapterCompleted(Guid userId, Guid chapterId, bool isCompleted)
    {
        var existing = _context.ChapterProgress
            .FirstOrDefault(p => p.UserId == userId && p.ChapterId == chapterId);

        if (existing is not null)
        {
            existing.LastViewedAt = DateTime.UtcNow;
            existing.IsCompleted = isCompleted;
        }
        else
        {
            _context.ChapterProgress.Add(new ChapterProgress
            {
                UserId = userId,
                ChapterId = chapterId,
                IsCompleted = isCompleted,
                LastViewedAt = DateTime.UtcNow
            });
        }

        _context.SaveChanges();
    }
    
    public ProgressSummaryDto GetProgressSummary(Guid userId)
    {
        var allChapters = _context.Chapters
            .Include(c => c.Sections)
            .OrderBy(c => c.Order)
            .ToList();

        var sectionProgress = _context.SectionProgress
            .Where(p => p.UserId == userId && p.IsCompleted)
            .ToList();

        var chapterProgress = new List<ChapterProgressDetail>();
        int completedChapters = 0;

        foreach (var chapter in allChapters)
        {
            var totalSections = chapter.Sections.Count;
            if (totalSections == 0) continue;

            var completed = sectionProgress
                .Count(p => chapter.Sections.Select(s => s.Id).Contains(p.SectionId));

            var completionRate = Math.Round((double)completed / totalSections * 100, 2);

            if (completionRate == 100.0) completedChapters++;

            chapterProgress.Add(new ChapterProgressDetail
            {
                ChapterId = chapter.Id,
                Title = chapter.Title,
                CompletionRate = completionRate
            });
        }

        var totalChapters = allChapters.Count;
        var readinessScore = totalChapters == 0 ? 0 : Math.Round((double)completedChapters / totalChapters * 100, 2);

        return new ProgressSummaryDto
        {
            TotalChapters = totalChapters,
            CompletedChapters = completedChapters,
            ReadinessScore = readinessScore,
            ChaptersYouRock = chapterProgress.Where(c => c.CompletionRate >= 50).ToList(),
            ChaptersToImprove = chapterProgress.Where(c => c.CompletionRate < 50).ToList()
        };
    }


}
