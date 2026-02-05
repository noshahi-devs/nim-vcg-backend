using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models.DataModels
{
    [Table("Section")]
    public class Section
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SectionId { get; set; }

        [Required]
        public string SectionName { get; set; } = string.Empty;

        public string ClassName { get; set; } = string.Empty;

        public string SectionCode { get; set; } = string.Empty; // e.g. "A", "B"

        public int? StaffId { get; set; } // Class Teacher

        [ForeignKey("StaffId")]
        public virtual Staff? ClassTeacher { get; set; }

        public string RoomNo { get; set; } = string.Empty;

        public int Capacity { get; set; }
    }
}
