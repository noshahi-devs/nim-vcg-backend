namespace SchoolApp.Models.Email
{
    /// <summary>
    /// Email request model for sending emails
    /// </summary>
    public class EmailRequest
    {
        public string To { get; set; } = string.Empty;
        public string ToName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string HtmlBody { get; set; } = string.Empty;
        public string? PlainTextBody { get; set; }
        public List<EmailAttachment>? Attachments { get; set; }
        public string? ReplyTo { get; set; }
        public List<string>? Cc { get; set; }
        public List<string>? Bcc { get; set; }
        public Dictionary<string, string>? TemplateData { get; set; }
        public bool IsHighPriority { get; set; } = false;
    }

    /// <summary>
    /// Email attachment model
    /// </summary>
    public class EmailAttachment
    {
        public string FileName { get; set; } = string.Empty;
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "application/octet-stream";
    }

    /// <summary>
    /// Email response model
    /// </summary>
    public class EmailResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorDetails { get; set; }
        public DateTime SentAt { get; set; }
        public string? MessageId { get; set; }
    }

    /// <summary>
    /// Enum for notification event types
    /// </summary>
    public enum NotificationEvent
    {
        // Authentication
        LoginAlert,
        NewAccountCreation,
        PasswordResetRequest,

        // Academic
        ExamSchedulePublished,
        ExamDateUpdated,
        ResultAnnounced,
        ResultUpdated,

        // Leave
        LeaveApproved,
        LeaveRejected,

        // Assignment
        TeacherAssigned,
        ClassOrSectionChange,

        // Finance
        FeeVoucherGenerated,
        FeePaymentReceived,
        SalarySlipGenerated,

        // System
        AnnouncementPublished,
        MaintenanceAlert
    }
}
