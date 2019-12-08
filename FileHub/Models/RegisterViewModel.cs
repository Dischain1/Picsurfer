using System.ComponentModel.DataAnnotations;

namespace FileHub.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Введенные пароли отличаются")]
        public string ConfirmPassword { get; set; }
    }
}