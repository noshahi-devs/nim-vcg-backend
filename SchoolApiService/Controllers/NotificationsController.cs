using Microsoft.AspNetCore.Mvc;
using System.Linq;
using SchoolApiService.Services;
using SchoolApp.Models.Email;
using SchoolApp.DAL.SchoolContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SchoolApp.Models.DataModels.SecurityModels;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        public class BroadcastRequest
        {
            public string Title { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public string? Link { get; set; }
            public string? NotificationType { get; set; }
        }

        private readonly IEmailService _emailService;
        private readonly SchoolDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationsController(IEmailService emailService, SchoolDbContext context, UserManager<ApplicationUser> userManager)
        {
            _emailService = emailService;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)
                                   .Select(c => c.Value)
                                   .FirstOrDefault(v => Guid.TryParse(v, out _));
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(50)
                .ToListAsync();

            return Ok(notifications);
        }

        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)
                                   .Select(c => c.Value)
                                   .FirstOrDefault(v => Guid.TryParse(v, out _));
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (notification == null) return NotFound();

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("broadcast")]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> BroadcastNotification([FromBody] BroadcastRequest broadcast)
        {
            var userId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)
                                   .Select(c => c.Value)
                                   .FirstOrDefault(v => Guid.TryParse(v, out _));
            var allUserIds = await _context.Users.Select(u => u.Id).ToListAsync();
            var now = DateTime.UtcNow;

            var notifications = allUserIds.Select(uid => new Notification
            {
                UserId = uid,
                Title = broadcast.Title,
                Message = broadcast.Message,
                Link = broadcast.Link,
                NotificationType = broadcast.NotificationType ?? "Broadcast",
                CreatedAt = now,
                IsRead = false
            }).ToList();

            _context.Notifications.AddRange(notifications);

            // Also add to UserMessages so it appears in the sender's "Sent" folder
            // and potentially in recipients' inbox if we bridge them
            var broadcastMessage = new UserMessage
            {
                SenderId = userId ?? "System",
                ReceiverId = "All Users", // Placeholder for broadcast
                Subject = broadcast.Title,
                Content = broadcast.Message,
                CreatedAt = now,
                IsRead = false,
                IsStarred = false,
                IsDeletedIn = true, // Don't show in "All Users" inbox (since it's a placeholder)
                IsDeletedOut = false
            };
            _context.UserMessages.Add(broadcastMessage);

            await _context.SaveChangesAsync();
            return Ok(new { Count = notifications.Count });
        }

        [HttpGet("logs")]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<IActionResult> GetNotificationLogs()
        {
            try
            {
                // Group by Title, Message and CreatedAt (trimmed to seconds) to show each broadcast only once
                var logs = await _context.Notifications
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();
                
                var groupedLogs = logs
                    .GroupBy(n => new { n.Title, n.Message, CreatedDate = n.CreatedAt.AddTicks(-(n.CreatedAt.Ticks % TimeSpan.TicksPerSecond)) })
                    .Select(g => g.First())
                    .Take(50)
                    .Select(n => new
                    {
                        n.Id,
                        n.Title,
                        n.Message,
                        n.NotificationType,
                        n.CreatedAt,
                        n.IsRead
                    })
                    .ToList();

                return Ok(groupedLogs);
            }
            catch (Exception ex)
            {
                return Ok(new List<object>()); 
            }
        }

        [HttpPost("send-custom")]
        public async Task<IActionResult> SendCustomEmail(EmailRequest request)
        {
            var response = await _emailService.SendEmailAsync(request);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            // Simple test email
            var request = new EmailRequest
            {
                To = "admin@noshahi.edu.pk",
                ToName = "Admin",
                Subject = "SMTP Connection Test",
                HtmlBody = "<h1>Success</h1><p>The SMTP connection is working correctly.</p>",
                IsHighPriority = true
            };

            var response = await _emailService.SendEmailAsync(request);
            return Ok(response);
        }
    }
}
