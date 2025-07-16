using System;
using System.Threading.Tasks;
using HelloWorldApi.Data;
using HelloWorldApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TB_Social_Media.DTO;

namespace TB_Social_Media.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreateUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Username and Email are required");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User added successfully", userId = user.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
            => Ok(await _context.Users.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? NotFound("User not found.") : Ok(user);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound("User not found");

            user.Username = request.Username;
            await _context.SaveChangesAsync();
            return Ok(new { message = "User updated successfully" });
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "User deleted successfully" });
        }
    }
}