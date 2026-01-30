using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models.DataModels
{
    [Table("GeneralIncome")]
    public class GeneralIncome
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Source { get; set; } = string.Empty; // e.g., Donation, Rent
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "Cash";
        public string ReceivedBy { get; set; } = string.Empty;
        public string Campus { get; set; } = "Main Campus";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
