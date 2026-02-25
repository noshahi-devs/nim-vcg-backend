using SchoolApp.DAL.SchoolContext;
using SchoolApp.Models.DataModels;

namespace SchoolApiService.Services
{
    public interface INotificationService
    {
        Task LogEventAsync(string recipientName, string recipientEmail, string type, string status, string message = "");
    }

    public class NotificationService(SchoolDbContext context) : INotificationService
    {
        private readonly SchoolDbContext _context = context;

        public async Task LogEventAsync(string recipientName, string recipientEmail, string type, string status, string message = "")
        {
            var log = new NotificationLog
            {
                RecipientName = recipientName,
                RecipientEmail = recipientEmail,
                Subject = type,
                HtmlBody = message,
                NotificationType = type,
                Status = status,
                ErrorMessage = status == "Failed" ? message : null,
                CreatedAt = DateTime.UtcNow
            };

            _context.NotificationLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
