using System;
using System.Linq;
using System.Threading.Tasks;
using HelloWorldApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TB_Social_Media.DTO;
using TB_Social_Media.Models;

namespace TB_Social_Media.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CommentsController(AppDbContext context)
            => _context = context;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest("Content is required.");

            if (!await _context.Users.AnyAsync(u => u.Id == dto.UserId))
                return BadRequest("User not found.");
            if (!await _context.Posts.AnyAsync(p => p.Id == dto.PostId))
                return BadRequest("Post not found.");

            var comment = new Comment
            {
                Content   = dto.Content,
                UserId    = dto.UserId,
                PostId    = dto.PostId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Comment created.", commentId = comment.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Post)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var c = await _context.Comments
                .Include(x => x.User)
                .Include(x => x.Post)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (c == null) return NotFound("Comment not found.");
            return Ok(c);
        }

        [HttpGet("byPost/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(int postId)
        {
            var comments = await _context.Comments
            .Where(c => c.PostId == postId)
            .ToListAsync();

            if (comments == null || comments.Count == 0)
                return NotFound("No comments found for this post");

                return Ok(comments);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommentRequest dto)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound("Comment not found.");
            if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest("Content is required.");

            comment.Content = dto.Content;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Comment updated." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound("Comment not found.");

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Comment deleted." });
        }
    }
}