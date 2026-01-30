using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models.DataModels
{
    [Table("GeneralExpense")]
    public class GeneralExpense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string ExpenseType { get; set; } = "Other"; // e.g., Bill, Purchase
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "Cash";
        public string PaidTo { get; set; } = string.Empty;
        public string ApprovedBy { get; set; } = string.Empty;
        public string Campus { get; set; } = "Main Campus";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
