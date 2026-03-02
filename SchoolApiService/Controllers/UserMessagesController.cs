using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;
using SchoolApp.Models.DataModels.SecurityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserMessagesController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserMessagesController(SchoolDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("inbox")]
        public async Task<IActionResult> GetInbox()
        {
            var userId = _userManager.GetUserId(User);
            var messages = await _context.UserMessages
                .Where(m => m.ReceiverId == userId && !m.IsDeletedIn)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            return Ok(messages);
        }

        [HttpGet("sent")]
        public async Task<IActionResult> GetSent()
        {
            var userId = _userManager.GetUserId(User);
            var messages = await _context.UserMessages
                .Where(m => m.SenderId == userId && !m.IsDeletedOut)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] UserMessage message)
        {
            var userId = _userManager.GetUserId(User);
            message.SenderId = userId!;
            message.CreatedAt = DateTime.UtcNow;
            message.IsRead = false;
            message.IsStarred = false;
            message.IsDeletedIn = false;
            message.IsDeletedOut = false;

            _context.UserMessages.Add(message);
            await _context.SaveChangesAsync();
            return Ok(message);
        }

        [HttpPatch("{id}/star")]
        public async Task<IActionResult> ToggleStar(int id)
        {
            var userId = _userManager.GetUserId(User);
            var message = await _context.UserMessages
                .FirstOrDefaultAsync(m => m.Id == id && (m.SenderId == userId || m.ReceiverId == userId));

            if (message == null) return NotFound();

            message.IsStarred = !message.IsStarred;
            await _context.SaveChangesAsync();
            return Ok(new { IsStarred = message.IsStarred });
        }

        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = _userManager.GetUserId(User);
            var message = await _context.UserMessages
                .FirstOrDefaultAsync(m => m.Id == id && m.ReceiverId == userId);

            if (message == null) return NotFound();

            message.IsRead = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var userId = _userManager.GetUserId(User);
            var message = await _context.UserMessages
                .FirstOrDefaultAsync(m => m.Id == id && (m.SenderId == userId || m.ReceiverId == userId));

            if (message == null) return NotFound();

            if (message.SenderId == userId) message.IsDeletedOut = true;
            if (message.ReceiverId == userId) message.IsDeletedIn = true;

            // If both deleted, we could purge from DB, but keeping for now
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
