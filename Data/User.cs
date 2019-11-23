using System.ComponentModel.DataAnnotations;

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

        public string PasswordHash { get; set; }

        public bool IsAdmin { get; set; }
    }
}
