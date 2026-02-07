namespace SchoolApp.Models.Email
{
    /// <summary>
    /// Email settings configuration from appsettings.json
    /// </summary>
    public class EmailSettings
    {
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public bool EnableSsl { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderPassword { get; set; } = string.Empty;
        public string DefaultReplyTo { get; set; } = string.Empty;
        public int MaxRetryAttempts { get; set; } = 3;
        public int RetryDelaySeconds { get; set; } = 5;
        public bool EnableEmailLogging { get; set; } = true;
    }

    /// <summary>
    /// Notification settings for controlling which events trigger emails
    /// </summary>
    public class NotificationSettings
    {
        public bool EnableNotifications { get; set; }
        public AuthenticationNotifications AuthenticationNotifications { get; set; } = new();
        public AcademicNotifications AcademicNotifications { get; set; } = new();
        public LeaveNotifications LeaveNotifications { get; set; } = new();
        public AssignmentNotifications AssignmentNotifications { get; set; } = new();
        public FinanceNotifications FinanceNotifications { get; set; } = new();
        public SystemNotifications SystemNotifications { get; set; } = new();
    }

    public class AuthenticationNotifications
    {
        public bool SendLoginAlert { get; set; }
        public bool SendNewAccountCreation { get; set; }
        public bool SendPasswordResetRequest { get; set; }
    }

    public class AcademicNotifications
    {
        public bool SendExamSchedulePublished { get; set; }
        public bool SendExamDateUpdated { get; set; }
        public bool SendResultAnnounced { get; set; }
        public bool SendResultUpdated { get; set; }
    }

    public class LeaveNotifications
    {
        public bool SendLeaveApproved { get; set; }
        public bool SendLeaveRejected { get; set; }
    }

    public class AssignmentNotifications
    {
        public bool SendTeacherAssigned { get; set; }
        public bool SendClassOrSectionChange { get; set; }
    }

    public class FinanceNotifications
    {
        public bool SendFeeVoucherGenerated { get; set; }
        public bool SendFeePaymentReceived { get; set; }
        public bool SendSalarySlipGenerated { get; set; }
    }

    public class SystemNotifications
    {
        public bool SendAnnouncements { get; set; }
        public bool SendMaintenanceAlerts { get; set; }
    }

    /// <summary>
    /// Institute information for email branding
    /// </summary>
    public class InstituteInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
    }
}
