using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeCare.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public int ServiceCount { get; set; }
    }
}
