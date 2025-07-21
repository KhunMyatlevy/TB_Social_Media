using System;
using System.Linq;
using System.Threading.Tasks;
using HelloWorldApi.Data;
using Microsoft.AspNetCore.Authorization;
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
        public CommentsController(AppDbContext context) => _context = context;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest("Content is required.");

            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("Invalid or missing user ID.");

            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                return BadRequest("User not found.");

            if (!await _context.Posts.AnyAsync(p => p.Id == dto.PostId))
                return BadRequest("Post not found.");

            var comment = new Comment
            {
                Content = dto.Content,
                UserId = userId,
                PostId = dto.PostId,
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
            var comment = await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
                return NotFound("Comment not found.");

            return Ok(comment);
        }

        [HttpGet("byPost/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(int postId)
        {
            var comments = await _context.Comments
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            if (comments == null || comments.Count == 0)
                return NotFound("No comments found for this post.");

            return Ok(comments);
        }

        [HttpGet("byUser/{userId}")]
        public async Task<IActionResult> GetCommentsByUserId(int userId)
        {
            var comments = await _context.Comments
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            if (comments == null || comments.Count == 0)
                return NotFound("No comments found for this user.");

            return Ok(comments);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommentRequest dto)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound("Comment not found.");

            int userId = int.Parse(User.FindFirst("userId").Value);
            if (comment.UserId != userId)
                return Forbid("You can only edit your own comment.");

                if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest("Content is required.");

            comment.Content = dto.Content;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Comment updated." });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound("Comment not found.");

            int userId = int.Parse(User.FindFirst("userId").Value);
            if (comment.UserId != userId)
                return Forbid("You can only delete your own comment.");

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Comment deleted." });
        }
    }
}