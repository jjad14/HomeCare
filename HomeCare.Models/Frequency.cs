using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeCare.Models
{
    public class Frequency
    {
        public int Id { get; set; }

        [Required]
        public int FrequencyCount { get; set; }

        [Required]
        [Display(Name = "Frequency Name")]
        public string Name { get; set; }
    }
}
