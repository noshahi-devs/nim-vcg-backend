using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Models.DataModels
{
    [Table("LeaveTypeMasters")]
    public class LeaveTypeMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveTypeMasterId { get; set; }

        [Required]
        [StringLength(100)]
        public string LeaveTypeName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public int MaxDaysAllowed { get; set; } = 10;

        public bool IsPaid { get; set; } = true;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}
