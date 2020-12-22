using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeCare.Models
{
    public class WebImages
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // save the images on the database and not on the server
        public byte[] Image { get; set; }
    }
}
