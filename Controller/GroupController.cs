using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TB_Social_Media.Models;
using TB_Social_Media.DTO;
using HelloWorldApi.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

    namespace TB_Social_Media.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class GroupController : ControllerBase
        {
            private readonly AppDbContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public GroupController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
            }

            private int GetUserId()
            {
                return int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }

            [HttpPost]
            [Authorize]
            public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
            {
                var userId = GetUserId();

                var group = new Group
                {
                    Name = request.Name,
                    OwnerId = userId
                };

                _context.Groups.Add(group);
                await _context.SaveChangesAsync();

                return Ok(group);
            }

            [HttpPut("{groupId}")]
            [Authorize]
            public async Task<IActionResult> UpdateGroup(int groupId, [FromBody] UpdateGroupRequest request)
            {
                var userId = GetUserId();

                var group = await _context.Groups.FindAsync(groupId);
                if (group == null) return NotFound();

                if (group.OwnerId != userId) return Forbid("Only admin can update the group.");

                group.Name = request.Name;
                await _context.SaveChangesAsync();

                return Ok(group);
            }

            [HttpPost("{groupId}/posts")]
            [Authorize]
            public async Task<IActionResult> AddPost(int groupId, [FromBody] CreatePostRequest request)
            {
                var userId = GetUserId();

                var group = await _context.Groups.FindAsync(groupId);
                if (group == null) return NotFound();

                var post = new Post
                {
                    GroupId = groupId,
                    UserId = userId,
                    Content = request.Content
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                return Ok(post);
            }
        }
    }
   