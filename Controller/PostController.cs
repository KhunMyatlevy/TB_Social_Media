using System;
using System.Security.Claims;
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
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PostController(AppDbContext context) => _context = context;

        // ------------------  POST /api/post  ------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest dto)
        {
            int userId = GetUserIdFromToken();
            var post = new Post
            {
                Content   = dto.Content,
                UserId    = userId,
                Createdat = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Post created", postId = post.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _context.Posts
                                .OrderByDescending(p => p.Createdat)
                                .ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => await _context.Posts.FindAsync(id) is { } post
                   ? Ok(post)
                   : NotFound("Post not found");


        [HttpGet("byUser/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var posts = await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
            return posts.Count == 0
                   ? NotFound("No posts found for this user")
                   : Ok(posts);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePostRequest dto)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) reusing System.Threading.Tasks;turn NotFound("Post not found");

            int userId = GetUserIdFromToken();
            if (post.UserId != userId) return Forbid("You can only update your own post");

            post.Content = dto.Content;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Post updated" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound("Post not found");

            int userId = GetUserIdFromToken();
            if (post.UserId != userId) return Forbid("You can only delete your own post");

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Post deleted" });
        }

        private int GetUserIdFromToken()
        {
            var claim = User.FindFirst("userId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(claim?.Value ?? "0");
        }
    }
}