using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TB_Social_Media.Models;
using TB_Social_Media.DTO;
using HelloWorldApi.Data;

[ApiController]
[Route("api/[controller]")]
public class GroupController : ControllerBase
{
    private readonly AppDbContext _context;

    public GroupController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] UpdateGroupRequest request)
    {
        var group = new Group
        {
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();


        return Ok(group);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(int id, [FromBody] UpdateGroupRequest request)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null)
            return NotFound();

        group.Name = request.Name;
        await _context.SaveChangesAsync();


        return Ok(group);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroup(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null)
            return NotFound();

        return Ok(group);
    }

    [HttpPost("{groupId}/posts")]
    public async Task<IActionResult> CreatePost(int groupId, [FromBody] CreatePostRequest request)
    {
        var group = await _context.Groups.FindAsync(groupId);
        if (group == null)
            return NotFound("Group not found");


        var post = new Post
        {
            GroupId = groupId,
            UserId = request.UserId,
            Content = request.Content,
            Createdat = DateTime.UtcNow
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return Ok(post);
    }
}