using System;
using System.ComponentModel.DataAnnotations;

namespace BasicBilling.API.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}