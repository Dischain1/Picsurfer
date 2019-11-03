using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class User
    {
        [Key]
        public int Id{ get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string SecondName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Patronimic { get; set; }

        public bool IsAdmin { get; set; }
    }
}
