using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloWorldApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TB_Social_Media.DTO;
using TB_Social_Media.Models;

namespace TB_Social_Media.Controller
{
[Authorize]    
[ApiController]
[Route("api/[controller]")]
public class PageController : ControllerBase
{
    private readonly AppDbContext _context;

    public PageController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPage(int id)
    {
        var page = await _context.Pages.Include(p => p.Owner).FirstOrDefaultAsync(p => p.Id == id);
        if (page == null)
            return NotFound();

        return Ok(page);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePage([FromBody] CreatePageRequest dto)
    {
        var userId = int.Parse(User.FindFirst("userId").Value);
        var page = new Page
        {
            Title = dto.Title,
            Description = dto.Description,
            OwnerId = userId
        };

        _context.Pages.Add(page);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPage), new { id = page.Id }, page);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePage(int id, [FromBody] UpdatePageRequest dto)
    {
        var page = await _context.Pages.FindAsync(id);
        if (page == null)
            return NotFound();

        var userId = int.Parse(User.FindFirst("userId").Value);
        if (page.OwnerId != userId)
            return Forbid();

        page.Title = dto.Title;
        page.Description = dto.Description;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePage(int id)
    {
        var page = await _context.Pages.FindAsync(id);
        if (page == null)
            return NotFound();

        var userId = int.Parse(User.FindFirst("userId").Value);
        if (page.OwnerId != userId)
            return Forbid();

        _context.Pages.Remove(page);
        await _context.SaveChangesAsync();
        return NoContent();
    }

        [HttpPost("page/{pageId}/posts")]
    public async Task<IActionResult> CreatePostForPage(int pageId, [FromBody] CreatePostRequest dto)
    {
        var page = await _context.Pages.FindAsync(pageId);
        if (page == null)
            return NotFound(new { message = "Page not found" });

        var userId = int.Parse(User.FindFirst("userId").Value);

        var post = new Post
        {
            Content = dto.Content,
            PageId = pageId,
            OwnerId = userId,
            UserId = userId
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return Ok(post);
    }
}
}