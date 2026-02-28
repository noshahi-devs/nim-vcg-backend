using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models.DataModels
{
    [Table("SubjectAssignment")]
    public class SubjectAssignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectAssignmentId { get; set; }

        [Required]
        public int StaffId { get; set; }
        [ForeignKey("StaffId")]
        public virtual Staff? Staff { get; set; }

        [Required]
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject? Subject { get; set; }

        [Required]
        public int SectionId { get; set; }
        [ForeignKey("SectionId")]
        public virtual Section? Section { get; set; }
    }
}
