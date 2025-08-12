using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Automotive_Project.Common.EntityValidationConstants;
namespace Automotive_Project.Models
{
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(FirstNameMaxLength, ErrorMessage = "Max 50 characters allowed.")]
        public string FirstName { get; set; } 

        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(LastNameMaxLength, ErrorMessage = "Max 50 characters allowed.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(EmailAddressMaxLength, ErrorMessage = "Max 100 characters allowed.")]
        public string Email { get; set; } 

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(PasswordMaxLenght, ErrorMessage = "Max 20 or min 5 characters allowed.")]
        public string Password { get; set; }

        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }

    }
}
