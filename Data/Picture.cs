using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class Picture
    {
        [Key]
        public int Id{ get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        public string Extension { get; set; }

        public virtual List<Rate> Rates { get; set; }
    }
}
