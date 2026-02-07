using SchoolApp.Models.Email;

namespace SchoolApiService.Services
{
    /// <summary>
    /// Interface for Email Service
    /// </summary>
    public interface IEmailService
    {
        Task<EmailResponse> SendEmailAsync(EmailRequest request);
        Task<EmailResponse> SendEmailWithTemplateAsync(EmailRequest request, string templateName);
        Task<bool> SendBulkEmailsAsync(List<EmailRequest> requests);
        Task<EmailResponse> SendNotificationEmailAsync(NotificationEvent eventType, string recipientEmail, string recipientName, Dictionary<string, string> data);
    }
}
