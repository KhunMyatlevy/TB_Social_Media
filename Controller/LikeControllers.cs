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
    public class LikeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LikeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("give")]
        public async Task<IActionResult> GiveLike([FromBody] LikeRequest request)
        {
            var existingLike = await _context.Likes
            .FirstOrDefaultAsync(l => l.UserId == request.UserId && l.PostId == request.PostId);

            if (existingLike != null)
                return BadRequest("You already liked this post");

            var like = new Like
            {
                UserId = request.UserId,
                PostId = request.PostId
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return Ok("Post liked successfully");
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveLike([FromBody] LikeRequest request)
        {
            var existingLike = await _context.Likes
            .FirstOrDefaultAsync(l => l.UserId == request.UserId && l.PostId == request.PostId);

            if (existingLike == null)
                return NotFound("Like not found");

            _context.Likes.Remove(existingLike);
            await _context.SaveChangesAsync();
            
            return Ok("Like removed successfully.");
        }
    }
}