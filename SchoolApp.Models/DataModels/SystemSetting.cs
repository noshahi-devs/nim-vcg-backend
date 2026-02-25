using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models.DataModels
{
    [Table("SystemSettings")]
    public class SystemSetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string SettingKey { get; set; } = string.Empty;

        [Required]
        public string SettingValue { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Category { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
