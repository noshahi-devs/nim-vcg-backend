using Microsoft.AspNetCore.Mvc;
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
            var userId = _userManager.GetUserId(User);
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
            var userId = _userManager.GetUserId(User);
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (notification == null) return NotFound();

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("broadcast")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BroadcastNotification([FromBody] Notification broadcast)
        {
            var allUserIds = await _context.Users.Select(u => u.Id).ToListAsync();
            var notifications = allUserIds.Select(uid => new Notification
            {
                UserId = uid,
                Title = broadcast.Title,
                Message = broadcast.Message,
                Link = broadcast.Link,
                NotificationType = "Broadcast",
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            }).ToList();

            _context.Notifications.AddRange(notifications);
            await _context.SaveChangesAsync();
            return Ok(new { Count = notifications.Count });
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetNotificationLogs()
        {
            var logs = await _context.NotificationLogs
                .OrderByDescending(l => l.CreatedAt)
                .Take(100)
                .ToListAsync();
            return Ok(logs);
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
