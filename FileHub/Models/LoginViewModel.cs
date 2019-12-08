using System.ComponentModel.DataAnnotations;

namespace FileHub.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}