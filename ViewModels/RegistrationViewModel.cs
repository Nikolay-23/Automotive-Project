using System.ComponentModel.DataAnnotations;
using static Automotive_Project.Common.EntityValidationConstants;

namespace Automotive_Project.ViewModels
{
    public class RegistrationViewModel
    {

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(EmailAddressMaxLength)]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please Enter Valid Email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(PasswordMaxLenght, MinimumLength = PasswordMinLenght, ErrorMessage = "Max 20 or min 5 characters allowed.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Please confirm your password")]
        public string ConfirmPassword { get; set; }

    }
}
