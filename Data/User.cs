﻿using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class User
    {
        [Key]
        public int Id{ get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}
