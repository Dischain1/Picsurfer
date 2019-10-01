using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class User
    {
        public int Id{ get; set; }

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        public string SecondName { get; set; }
        [Required]
        [MaxLength(30)]
        public string Patronimic { get; set; }

        public bool IsAdmin { get; set; }
    }
}
