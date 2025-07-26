using System.Security.Claims;
using DriveSmart.Application.Services;
using DriveSmart.Shared.Theory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriveSmart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TheoryController : AppControllerBase
{
    private readonly TheoryService _service;

    public TheoryController(TheoryService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet("chapters")]
    public ActionResult<List<ChapterDto>> GetChapters()
    {
        var chapters = _service.GetChapters(GetUserId());
        return Ok(chapters);
    }
    
    [HttpGet("chapters/{id}")]
    public ActionResult<ChapterDetailDto> GetChapterById(Guid id)
    {
        var chapter = _service.GetChapterById(GetUserId(), id);
        if (chapter == null) return NotFound();
        return Ok(chapter);
    }
    
    [Authorize]
    [HttpPost("progress/section")]
    public IActionResult TrackSection([FromBody] SectionProgressDto dto)
    {
        _service.MarkSectionViewed(GetUserId(), dto.SectionId, dto.IsCompleted);
        return Ok();
    }
    
    [Authorize]
    [HttpPost("progress/chapter")]
    public IActionResult TrackChapter([FromBody] ChapterProgressDto dto)
    {
        _service.MarkChapterCompleted(GetUserId(), dto.ChapterId, dto.IsCompleted);
        return Ok();
    }
    
    [Authorize]
    [HttpGet("progress/summary")]
    public ActionResult<ProgressSummaryDto> GetProgressSummary()
    {
        var summary = _service.GetProgressSummary(GetUserId());
        return Ok(summary);
    }
}