using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Automotive_Project.Common.EntityValidationConstants;
namespace Automotive_Project.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Password), IsUnique = true)]
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(EmailAddressMaxLength)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(PasswordMaxLenght)]
        public string Password { get; set; } = null!;
    }
}
