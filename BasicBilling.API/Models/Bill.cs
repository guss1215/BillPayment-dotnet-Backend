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
        public string ServiceType { get; set; }

        [Required]
        public string MonthYear { get; set; }

        [Required]
        public bool IsPaid { get; set; } // Pending or Paid
    }
}