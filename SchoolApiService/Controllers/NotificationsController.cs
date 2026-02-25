using Microsoft.AspNetCore.Mvc;
using SchoolApiService.Services;
using SchoolApp.Models.Email;
using SchoolApp.DAL.SchoolContext;
using Microsoft.EntityFrameworkCore;

namespace SchoolApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController(IEmailService emailService, SchoolDbContext context) : ControllerBase
    {
        private readonly IEmailService _emailService = emailService;
        private readonly SchoolDbContext _context = context;

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
