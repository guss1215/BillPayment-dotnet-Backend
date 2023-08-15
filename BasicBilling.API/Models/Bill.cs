using System;
using System.ComponentModel.DataAnnotations;

namespace BasicBilling.API.Models
{
    public class Bill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public int Period { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string PaymentStatus { get; set; }
    }
}
