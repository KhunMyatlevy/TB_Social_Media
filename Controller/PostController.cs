using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HelloWorldApi.Data;
using TB_Social_Media.DTO;
using TB_Social_Media.Models;
using Microsoft.EntityFrameworkCore;

namespace TB_Social_Media.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            var post = new Post
            {
                Content = request.Content,
                UserId = request.UserId
            };

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Post created" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _context.Posts
            .OrderByDescending(p => p.Createdat)
            .ToListAsync();

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound("Post not found");

            return Ok(post);
        }

        [HttpGet("byUser/{userId}")]
        public async Task<IActionResult> GetPostsByUserId(int userId)
        {
            var posts = await _context.Posts
            .Where(p => p.UserId == userId)
            .ToListAsync();

            if (posts == null || posts.Count == 0)
                return NotFound("No posts found for this user");

            return Ok(posts);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostRequest request)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            post.Content = request.Content;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Post updated." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Post deleted." });
        }
    }
}