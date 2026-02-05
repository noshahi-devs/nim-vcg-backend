using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Models.DataModels
{
    [Table("PaymentGatewaySettings")]
    public class PaymentGatewaySetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string GatewayName { get; set; } = string.Empty; // Stripe, PayPal

        [StringLength(200)]
        public string ApiKey { get; set; } = string.Empty;

        [StringLength(200)]
        public string SecretKey { get; set; } = string.Empty;

        public bool IsActive { get; set; } = false;

        public bool IsTestMode { get; set; } = true;

        public decimal TransactionFee { get; set; } = 0;
    }
}
