using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models.DataModels
{
    [Table("GradeScales")]
    public class GradeScale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GradeId { get; set; }

        [Required]
        [StringLength(10)]
        public string Grade { get; set; }

        [Required]
        public decimal MinPercentage { get; set; }

        [Required]
        public decimal MaxPercentage { get; set; }

        public decimal? GradePoint { get; set; }

        public string? Remarks { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
