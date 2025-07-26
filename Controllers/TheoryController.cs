using Drivia.Services;
using Drivia.Theory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Drivia.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TheoryController(TheoryService service) : AppControllerBase
{
    [Authorize]
    [HttpGet("chapters")]
    public ActionResult<List<ChapterDto>> GetChapters()
    {
        var chapters = service.GetChapters(GetUserId());
        return Ok(chapters);
    }
    
    [HttpGet("chapters/{id}")]
    public ActionResult<ChapterDetailDto> GetChapterById(Guid id)
    {
        var chapter = service.GetChapterById(GetUserId(), id);
        if (chapter == null) return NotFound();
        return Ok(chapter);
    }
    
    [Authorize]
    [HttpPost("progress/section")]
    public IActionResult TrackSection([FromBody] SectionProgressDto dto)
    {
        service.MarkSectionViewed(GetUserId(), dto.SectionId, dto.IsCompleted);
        return Ok();
    }
    
    [Authorize]
    [HttpPost("progress/chapter")]
    public IActionResult TrackChapter([FromBody] ChapterProgressDto dto)
    {
        service.MarkChapterCompleted(GetUserId(), dto.ChapterId, dto.IsCompleted);
        return Ok();
    }
    
    [Authorize]
    [HttpGet("progress/summary")]
    public ActionResult<ProgressSummaryDto> GetProgressSummary()
    {
        var summary = service.GetProgressSummary(GetUserId());
        return Ok(summary);
    }
}