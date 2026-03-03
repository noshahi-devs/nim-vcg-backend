using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolApp.Models.DataModels.SecurityModels;

namespace SchoolApp.Models.DataModels
{
    [Table("Parents")]
    public class Parent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParentId { get; set; }

        [Required]
        public string ParentName { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [NotMapped]
        public ICollection<Student> Children { get; set; } = new List<Student>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? CampusId { get; set; }

        [ForeignKey("CampusId")]
        public Campus? Campus { get; set; }
    }
}
