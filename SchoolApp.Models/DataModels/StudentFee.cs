using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models.DataModels
{
    [Table("StudentFees")]
    public class StudentFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentFeeId { get; set; }

        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        public int FeeId { get; set; }

        [ForeignKey("FeeId")]
        public Fee? Fee { get; set; }

        public decimal AssignedAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
