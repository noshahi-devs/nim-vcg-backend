using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models.DataModels
{
    /// <summary>
    /// Database model for logging all email notifications
    /// </summary>
    [Table("NotificationLogs")]
    public class NotificationLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string RecipientEmail { get; set; } = string.Empty;

        [MaxLength(200)]
        public string RecipientName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string HtmlBody { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string NotificationType { get; set; } = string.Empty; // e.g., "ExamSchedulePublished"

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty; // "Sent", "Failed", "Pending"

        public string? ErrorMessage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? SentAt { get; set; }

        public int RetryCount { get; set; } = 0;

        [MaxLength(200)]
        public string? MessageId { get; set; }

        // Foreign keys for linking to relevant entities
        public int? UserId { get; set; }
        public int? ExamId { get; set; }
        public int? StudentId { get; set; }
        public int? TeacherId { get; set; }
        public int? FeeId { get; set; }

        // Additional metadata
        public string? Metadata { get; set; } // JSON string for storing additional data
    }

    /// <summary>
    /// Database model for notification settings (admin can toggle)
    /// </summary>
    [Table("NotificationSettings")]
    public class NotificationSettingsDb
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string SettingKey { get; set; } = string.Empty; // e.g., "SendExamSchedulePublished"

        [Required]
        public bool IsEnabled { get; set; } = true;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty; // "Authentication", "Academic", etc.

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int? UpdatedBy { get; set; } // UserId who last updated
    }
}
