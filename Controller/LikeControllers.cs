using System;
using System.Threading.Tasks;
using HelloWorldApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TB_Social_Media.DTO;
using TB_Social_Media.Models;

namespace TB_Social_Media.Controllers
{
    [Authorize]                 
    [ApiController]
    [Route("api/[controller]")]
    public class LikeController : ControllerBase
    {
        private readonly AppDbContext _context;
        public LikeController(AppDbContext context) => _context = context;

        [HttpPost("give")]
        public async Task<IActionResult> GiveLike([FromBody] LikeRequest dto)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);

            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == dto.PostId);

            if (existingLike != null)
                return BadRequest("You already liked this post");

            _context.Likes.Add(new Like { UserId = userId, PostId = dto.PostId });
            await _context.SaveChangesAsync();

            return Ok(new { message = "Post liked successfully" });
        }

        [HttpDelete("remove/{postId:int}")]
        public async Task<IActionResult> RemoveLike(int postId)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);

            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);

            if (existingLike == null)
                return NotFound("Like not found");

            _context.Likes.Remove(existingLike);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Like removed successfully" });
        }
    }
}