using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class File
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

        [Required]
        public DateTime UploadDate { get; set; }

        public virtual List<Download> Downloads { get; set; }
    }
}
