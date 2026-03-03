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
            public List<string>? TargetRoles { get; set; }
            public List<int>? TargetSectionIds { get; set; }
            public List<int>? TargetSubjectIds { get; set; }
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
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<IActionResult> BroadcastNotification([FromBody] BroadcastRequest broadcast)
        {
            var senderRoles = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();
            
            // Diagnostic logging
            Console.WriteLine($"Broadcast attempt by: {User.Identity?.Name}");
            Console.WriteLine($"Roles found in token: {string.Join(", ", senderRoles)}");
            
            var isTeacher = senderRoles.Contains("Teacher") && !senderRoles.Contains("Admin") && !senderRoles.Contains("Principal");
            var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            var query = _context.Users.AsQueryable();

            if (isTeacher)
            {
                if (string.IsNullOrEmpty(email)) return Unauthorized();
                var staff = await _context.dbsStaff.FirstOrDefaultAsync(s => s.Email == email);
                if (staff == null) return Unauthorized("Staff record not found.");

                // Teachers can only broadcast to Students in their assigned sections
                var assignedSectionIds = await _context.SubjectAssignments
                    .Where(sa => sa.StaffId == staff.StaffId)
                    .Select(sa => sa.SectionId)
                    .Distinct()
                    .ToListAsync();

                var targetSections = broadcast.TargetSectionIds != null && broadcast.TargetSectionIds.Any() 
                    ? broadcast.TargetSectionIds.Intersect(assignedSectionIds).ToList() 
                    : assignedSectionIds;

                if (broadcast.TargetSubjectIds != null && broadcast.TargetSubjectIds.Any())
                {
                    // Find sections that have these subjects taught by the teacher
                    var relevantSectionIds = await _context.SubjectAssignments
                        .Where(sa => sa.StaffId == staff.StaffId && broadcast.TargetSubjectIds.Contains(sa.SubjectId))
                        .Select(sa => sa.SectionId)
                        .Distinct()
                        .ToListAsync();
                    
                    targetSections = targetSections.Intersect(relevantSectionIds).ToList();
                }

                var studentUserIds = await _context.dbsStudent
                    .Where(s => s.SectionId.HasValue && targetSections.Contains(s.SectionId.Value))
                    .Select(s => s.UserId)
                    .Where(uid => uid != null)
                    .Distinct()
                    .ToListAsync();

                query = query.Where(u => studentUserIds.Contains(u.Id));
            }
            else
            {
                // Admin/Principal logic
                List<string> filteredUserIds = new List<string>();

                if (broadcast.TargetSectionIds != null && broadcast.TargetSectionIds.Any())
                {
                    var studentUserIds = await _context.dbsStudent
                        .Where(s => s.SectionId.HasValue && broadcast.TargetSectionIds.Contains(s.SectionId.Value))
                        .Select(s => s.UserId)
                        .Where(uid => uid != null)
                        .Distinct()
                        .ToListAsync();
                    filteredUserIds.AddRange(studentUserIds!);
                }

                if (broadcast.TargetSubjectIds != null && broadcast.TargetSubjectIds.Any())
                {
                    var studentUserIds = await _context.dbsStudent
                        .Join(_context.SubjectAssignments, 
                             s => s.SectionId, 
                             sa => sa.SectionId, 
                             (s, sa) => new { s, sa })
                        .Where(x => x.s.SectionId.HasValue && broadcast.TargetSubjectIds.Contains(x.sa.SubjectId))
                        .Select(x => x.s.UserId)
                        .Where(uid => uid != null)
                        .Distinct()
                        .ToListAsync();
                     filteredUserIds.AddRange(studentUserIds!);
                }

                if (filteredUserIds.Any())
                {
                    query = query.Where(u => filteredUserIds.Contains(u.Id));
                }

                if (broadcast.TargetRoles != null && broadcast.TargetRoles.Any())
                {
                    List<string> roleUserIds = new List<string>();
                    foreach (var role in broadcast.TargetRoles)
                    {
                        var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                        roleUserIds.AddRange(usersInRole.Select(u => u.Id));
                    }
                    var uniqueIdsByRole = roleUserIds.Distinct().ToList();
                    query = query.Where(u => uniqueIdsByRole.Contains(u.Id));
                }
            }

            var targetUserIds = await query.Select(u => u.Id).ToListAsync();
            var now = DateTime.UtcNow;

            var notifications = targetUserIds.Select(uid => new Notification
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
            var userId = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)
                                   .Select(c => c.Value)
                                   .FirstOrDefault(v => Guid.TryParse(v, out _));

            var broadcastMessage = new UserMessage
            {
                SenderId = userId ?? "System",
                ReceiverId = isTeacher ? (broadcast.TargetSectionIds != null && broadcast.TargetSectionIds.Any() ? $"Sections: {string.Join(", ", broadcast.TargetSectionIds)}" : "All Assigned Students") : (broadcast.TargetRoles != null && broadcast.TargetRoles.Any() ? string.Join(", ", broadcast.TargetRoles) : "All Users"), // Placeholder for broadcast
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

        [HttpGet("my-sections")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetMySections()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var staff = await _context.dbsStaff.FirstOrDefaultAsync(s => s.Email == email);
            if (staff == null) return NotFound("Staff record not found.");

            var sections = await _context.SubjectAssignments
                .Where(sa => sa.StaffId == staff.StaffId)
                .Include(sa => sa.Section)
                    .ThenInclude(sec => sec.Campus)
                .Include(sa => sa.Subject)
                .Select(sa => new
                {
                    sa.Section.SectionId,
                    sa.Section.SectionName,
                    sa.Section.ClassName,
                    CampusName = sa.Section.Campus != null ? sa.Section.Campus.CampusName : "",
                    sa.SubjectId,
                    SubjectName = sa.Subject != null ? sa.Subject.SubjectName : ""
                })
                .Distinct()
                .ToListAsync();

            return Ok(sections);
        }

        [HttpGet("logs")]
        [Authorize(Roles = "Admin,Principal,Teacher")]
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
