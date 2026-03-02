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
            var userId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)
                                   .Select(c => c.Value)
                                   .FirstOrDefault(v => Guid.TryParse(v, out _));
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // Fetch standard messages
            var messages = await _context.UserMessages
                .Where(m => m.ReceiverId == userId && !m.IsDeletedIn)
                .Select(m => new UserMessage
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Subject = m.Subject,
                    Content = m.Content,
                    IsRead = m.IsRead,
                    IsStarred = m.IsStarred,
                    IsDeletedIn = m.IsDeletedIn,
                    IsDeletedOut = m.IsDeletedOut,
                    CreatedAt = m.CreatedAt
                })
                .ToListAsync();

            // Fetch broadcasts from notifications
            var broadcasts = await _context.Notifications
                .Where(n => n.UserId == userId && n.NotificationType == "Broadcast")
                .Select(n => new UserMessage
                {
                    Id = -n.Id, // negative ID to distinguish from real messages
                    SenderId = "System Broadcast",
                    ReceiverId = userId,
                    Subject = n.Title,
                    Content = n.Message,
                    IsRead = n.IsRead,
                    IsStarred = false,
                    IsDeletedIn = false,
                    IsDeletedOut = false,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();

            var combined = messages.Concat(broadcasts).OrderByDescending(m => m.CreatedAt).ToList();
            return Ok(combined);
        }

        [HttpGet("sent")]
        public async Task<IActionResult> GetSent()
        {
            var userId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)
                                   .Select(c => c.Value)
                                   .FirstOrDefault(v => Guid.TryParse(v, out _));
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var messages = await _context.UserMessages
                .Where(m => m.SenderId == userId && !m.IsDeletedOut)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] UserMessage message)
        {
            var userId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)
                                   .Select(c => c.Value)
                                   .FirstOrDefault(v => Guid.TryParse(v, out _));
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (message.ReceiverId == "System Broadcast")
            {
                // Find a default admin or principal to receive the reply
                var fallbackAdmin = await _userManager.GetUsersInRoleAsync("Admin");
                var recipient = fallbackAdmin.FirstOrDefault() ?? await _context.Users.FirstOrDefaultAsync();
                if (recipient != null)
                {
                    message.ReceiverId = recipient.Id;
                }
            }

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
            var userId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)
                                   .Select(c => c.Value)
                                   .FirstOrDefault(v => Guid.TryParse(v, out _));
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (id < 0)
            {
                // Stars aren't really supported in Notifications model yet, 
                // but we can return Ok for now to avoid errors, or implement NotificationStarring
                return Ok(new { IsStarred = false });
            }

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
            var userId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)
                                   .Select(c => c.Value)
                                   .FirstOrDefault(v => Guid.TryParse(v, out _));
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (id < 0)
            {
                var notification = await _context.Notifications
                    .FirstOrDefaultAsync(n => n.Id == -id && n.UserId == userId);
                if (notification == null) return NotFound();
                notification.IsRead = true;
                await _context.SaveChangesAsync();
                return Ok();
            }

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
            var userId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)
                                   .Select(c => c.Value)
                                   .FirstOrDefault(v => Guid.TryParse(v, out _));
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (id < 0)
            {
                // We can "delete" a notification by marking it as something, 
                // but let's just ignore for now or implement Notification.IsDeleted
                return Ok();
            }

            var message = await _context.UserMessages
                .FirstOrDefaultAsync(m => m.Id == id && (m.SenderId == userId || m.ReceiverId == userId));

            if (message == null) return NotFound();

            if (message.SenderId == userId) message.IsDeletedOut = true;
            if (message.ReceiverId == userId) message.IsDeletedIn = true;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
