using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using SchoolApp.Models.Email;
using SchoolApp.Models.DataModels;
using SchoolApp.DAL.SchoolContext;
using System.Text.Json;

namespace SchoolApiService.Services
{
    /// <summary>
    /// Production-ready Email Service with retry logic, logging, and template support
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly NotificationSettings _notificationSettings;
        private readonly InstituteInfo _instituteInfo;
        private readonly ILogger<EmailService> _logger;
        private readonly SchoolDbContext _dbContext;

        public EmailService(
            IOptions<EmailSettings> emailSettings,
            IOptions<NotificationSettings> notificationSettings,
            IOptions<InstituteInfo> instituteInfo,
            ILogger<EmailService> logger,
            SchoolDbContext dbContext)
        {
            _emailSettings = emailSettings.Value;
            _notificationSettings = notificationSettings.Value;
            _instituteInfo = instituteInfo.Value;
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Send a single email with automatic retry logic
        /// </summary>
        public async Task<EmailResponse> SendEmailAsync(EmailRequest request)
        {
            var response = new EmailResponse();
            int attemptCount = 0;

            while (attemptCount < _emailSettings.MaxRetryAttempts)
            {
                try
                {
                    attemptCount++;
                    _logger.LogInformation($"Attempting to send email (Attempt {attemptCount}/{_emailSettings.MaxRetryAttempts}) to: {request.To}");

                    using (var smtpClient = CreateSmtpClient())
                    using (var mailMessage = CreateMailMessage(request))
                    {
                        await smtpClient.SendMailAsync(mailMessage);

                        response.IsSuccess = true;
                        response.Message = "Email sent successfully";
                        response.SentAt = DateTime.UtcNow;
                        response.MessageId = mailMessage.Headers["Message-ID"];

                        _logger.LogInformation($"Email sent successfully to: {request.To}");

                        // Log to database
                        if (_emailSettings.EnableEmailLogging)
                        {
                            await LogEmailAsync(request, "Sent", null);
                        }

                        return response;
                    }
                }
                catch (SmtpException smtpEx)
                {
                    _logger.LogError(smtpEx, $"SMTP error sending email to {request.To} (Attempt {attemptCount})");
                    response.ErrorDetails = $"SMTP Error: {smtpEx.Message}";

                    if (attemptCount >= _emailSettings.MaxRetryAttempts)
                    {
                        response.IsSuccess = false;
                        response.Message = "Failed to send email after maximum retry attempts";
                        await LogEmailAsync(request, "Failed", smtpEx.Message);
                        break;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(_emailSettings.RetryDelaySeconds * attemptCount));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Unexpected error sending email to {request.To}");
                    response.IsSuccess = false;
                    response.Message = "Unexpected error occurred";
                    response.ErrorDetails = ex.Message;
                    await LogEmailAsync(request, "Failed", ex.Message);
                    break;
                }
            }

            return response;
        }

        /// <summary>
        /// Send email using a predefined HTML template
        /// </summary>
        public async Task<EmailResponse> SendEmailWithTemplateAsync(EmailRequest request, string templateName)
        {
            try
            {
                var templateHtml = await GetEmailTemplateAsync(templateName);

                if (request.TemplateData != null)
                {
                    foreach (var kvp in request.TemplateData)
                    {
                        templateHtml = templateHtml.Replace($"{{{{{kvp.Key}}}}}", kvp.Value);
                    }
                }

                // Add institute branding
                templateHtml = AddInstituteBranding(templateHtml);

                request.HtmlBody = templateHtml;
                return await SendEmailAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending templated email: {templateName}");
                return new EmailResponse
                {
                    IsSuccess = false,
                    Message = "Failed to send templated email",
                    ErrorDetails = ex.Message
                };
            }
        }

        /// <summary>
        /// Send bulk emails (e.g., to all students in a class)
        /// </summary>
        public async Task<bool> SendBulkEmailsAsync(List<EmailRequest> requests)
        {
            var tasks = requests.Select(SendEmailAsync);
            var results = await Task.WhenAll(tasks);
            return results.All(r => r.IsSuccess);
        }

        /// <summary>
        /// Send notification email based on event type
        /// </summary>
        public async Task<EmailResponse> SendNotificationEmailAsync(
            NotificationEvent eventType,
            string recipientEmail,
            string recipientName,
            Dictionary<string, string> data)
        {
            // Check if this notification type is enabled
            if (!IsNotificationEnabled(eventType))
            {
                _logger.LogInformation($"Notification {eventType} is disabled. Skipping email to {recipientEmail}");
                return new EmailResponse
                {
                    IsSuccess = false,
                    Message = $"Notification type {eventType} is disabled"
                };
            }

            var request = new EmailRequest
            {
                To = recipientEmail,
                ToName = recipientName,
                TemplateData = data
            };

            // Set subject and template based on event type
            var (subject, templateName) = GetSubjectAndTemplate(eventType, data);
            request.Subject = subject;

            return await SendEmailWithTemplateAsync(request, templateName);
        }

        #region Private Helper Methods

        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort)
            {
                EnableSsl = _emailSettings.EnableSsl,
                Credentials = new NetworkCredential(
                    _emailSettings.SenderEmail,
                    _emailSettings.SenderPassword),
                Timeout = 30000 // 30 seconds
            };
        }

        private MailMessage CreateMailMessage(EmailRequest request)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = request.Subject,
                Body = request.HtmlBody,
                IsBodyHtml = true,
                Priority = request.IsHighPriority ? MailPriority.High : MailPriority.Normal
            };

            mailMessage.To.Add(new MailAddress(request.To, request.ToName));

            if (!string.IsNullOrEmpty(request.ReplyTo))
            {
                mailMessage.ReplyToList.Add(new MailAddress(request.ReplyTo));
            }
            else
            {
                mailMessage.ReplyToList.Add(new MailAddress(_emailSettings.DefaultReplyTo));
            }

            if (request.Cc != null)
            {
                foreach (var cc in request.Cc)
                {
                    mailMessage.CC.Add(cc);
                }
            }

            if (request.Bcc != null)
            {
                foreach (var bcc in request.Bcc)
                {
                    mailMessage.Bcc.Add(bcc);
                }
            }

            if (request.Attachments != null)
            {
                foreach (var attachment in request.Attachments)
                {
                    var stream = new MemoryStream(attachment.Content);
                    mailMessage.Attachments.Add(new Attachment(stream, attachment.FileName, attachment.ContentType));
                }
            }

            return mailMessage;
        }

        private async Task LogEmailAsync(EmailRequest request, string status, string? errorMessage)
        {
            try
            {
                var log = new NotificationLog
                {
                    RecipientEmail = request.To,
                    RecipientName = request.ToName,
                    Subject = request.Subject,
                    HtmlBody = request.HtmlBody,
                    NotificationType = request.TemplateData?.GetValueOrDefault("EventType", "General") ?? "General",
                    Status = status,
                    ErrorMessage = errorMessage,
                    SentAt = status == "Sent" ? DateTime.UtcNow : null,
                    Metadata = request.TemplateData != null ? JsonSerializer.Serialize(request.TemplateData) : null
                };

                _dbContext.NotificationLogs.Add(log);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to log email to database");
            }
        }

        private bool IsNotificationEnabled(NotificationEvent eventType)
        {
            if (!_notificationSettings.EnableNotifications)
                return false;

            return eventType switch
            {
                NotificationEvent.LoginAlert => _notificationSettings.AuthenticationNotifications.SendLoginAlert,
                NotificationEvent.NewAccountCreation => _notificationSettings.AuthenticationNotifications.SendNewAccountCreation,
                NotificationEvent.PasswordResetRequest => _notificationSettings.AuthenticationNotifications.SendPasswordResetRequest,
                NotificationEvent.ExamSchedulePublished => _notificationSettings.AcademicNotifications.SendExamSchedulePublished,
                NotificationEvent.ExamDateUpdated => _notificationSettings.AcademicNotifications.SendExamDateUpdated,
                NotificationEvent.ResultAnnounced => _notificationSettings.AcademicNotifications.SendResultAnnounced,
                NotificationEvent.ResultUpdated => _notificationSettings.AcademicNotifications.SendResultUpdated,
                NotificationEvent.LeaveApproved => _notificationSettings.LeaveNotifications.SendLeaveApproved,
                NotificationEvent.LeaveRejected => _notificationSettings.LeaveNotifications.SendLeaveRejected,
                NotificationEvent.TeacherAssigned => _notificationSettings.AssignmentNotifications.SendTeacherAssigned,
                NotificationEvent.ClassOrSectionChange => _notificationSettings.AssignmentNotifications.SendClassOrSectionChange,
                NotificationEvent.FeeVoucherGenerated => _notificationSettings.FinanceNotifications.SendFeeVoucherGenerated,
                NotificationEvent.FeePaymentReceived => _notificationSettings.FinanceNotifications.SendFeePaymentReceived,
                NotificationEvent.SalarySlipGenerated => _notificationSettings.FinanceNotifications.SendSalarySlipGenerated,
                NotificationEvent.AnnouncementPublished => _notificationSettings.SystemNotifications.SendAnnouncements,
                NotificationEvent.MaintenanceAlert => _notificationSettings.SystemNotifications.SendMaintenanceAlerts,
                _ => true
            };
        }

        private (string Subject, string TemplateName) GetSubjectAndTemplate(NotificationEvent eventType, Dictionary<string, string> data)
        {
            return eventType switch
            {
                NotificationEvent.LoginAlert => ("New Login Alert - Noshahi Institute", "LoginAlert"),
                NotificationEvent.NewAccountCreation => ("Welcome to Noshahi Institute - Account Created", "NewAccountCreation"),
                NotificationEvent.PasswordResetRequest => ("Password Reset Request - Noshahi Institute", "PasswordReset"),
                NotificationEvent.ExamSchedulePublished => ($"Exam Schedule Published - {data.GetValueOrDefault("ExamName", "")}", "ExamSchedulePublished"),
                NotificationEvent.ExamDateUpdated => ($"Exam Date Updated - {data.GetValueOrDefault("ExamName", "")}", "ExamDateUpdated"),
                NotificationEvent.ResultAnnounced => ($"Results Announced - {data.GetValueOrDefault("ExamName", "")}", "ResultAnnounced"),
                NotificationEvent.ResultUpdated => ($"Results Updated - {data.GetValueOrDefault("ExamName", "")}", "ResultUpdated"),
                NotificationEvent.LeaveApproved => ("Leave Request Approved", "LeaveApproved"),
                NotificationEvent.LeaveRejected => ("Leave Request Rejected", "LeaveRejected"),
                NotificationEvent.TeacherAssigned => ("New Teaching Assignment", "TeacherAssigned"),
                NotificationEvent.ClassOrSectionChange => ("Class/Section Change Notification", "ClassSectionChange"),
                NotificationEvent.FeeVoucherGenerated => ($"Fee Voucher Generated - {data.GetValueOrDefault("Month", "")}", "FeeVoucherGenerated"),
                NotificationEvent.FeePaymentReceived => ("Fee Payment Received", "FeePaymentReceived"),
                NotificationEvent.SalarySlipGenerated => ($"Salary Slip - {data.GetValueOrDefault("Month", "")}", "SalarySlipGenerated"),
                NotificationEvent.AnnouncementPublished => ("Important Announcement - Noshahi Institute", "AnnouncementPublished"),
                NotificationEvent.MaintenanceAlert => ("System Maintenance Alert", "MaintenanceAlert"),
                _ => ("Notification from Noshahi Institute", "Default")
            };
        }

        private async Task<string> GetEmailTemplateAsync(string templateName)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", $"{templateName}.html");

            if (!File.Exists(templatePath))
            {
                _logger.LogWarning($"Template not found: {templateName}. Using default template.");
                return GetDefaultTemplate();
            }

            return await File.ReadAllTextAsync(templatePath);
        }

        private string AddInstituteBranding(string htmlContent)
        {
            return htmlContent
                .Replace("{{InstituteName}}", _instituteInfo.Name)
                .Replace("{{InstituteAddress}}", _instituteInfo.Address)
                .Replace("{{InstitutePhone}}", _instituteInfo.Phone)
                .Replace("{{InstituteEmail}}", _instituteInfo.Email)
                .Replace("{{InstituteWebsite}}", _instituteInfo.Website)
                .Replace("{{InstituteLogo}}", _instituteInfo.Logo);
        }

        private string GetDefaultTemplate()
        {
            return @"
<!DOCTYPE html>
<html>
<head><meta charset='UTF-8'></head>
<body style='font-family: Arial, sans-serif;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
        <div style='text-align: center; margin-bottom: 30px;'>
            <img src='{{InstituteLogo}}' alt='{{InstituteName}}' style='max-width: 150px;'>
        </div>
        <div>{{Content}}</div>
        <hr style='margin-top: 30px; border: 1px solid #eee;'>
        <p style='text-align: center; color: #666; font-size: 12px;'>
            {{InstituteName}}<br>
            {{InstituteAddress}}<br>
            Phone: {{InstitutePhone}} | Email: {{InstituteEmail}}<br>
            <a href='{{InstituteWebsite}}'>{{InstituteWebsite}}</a>
        </p>
    </div>
</body>
</html>";
        }

        #endregion
    }
}
