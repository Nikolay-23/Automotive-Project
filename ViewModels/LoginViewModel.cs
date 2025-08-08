using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Automotive_Project.Common.EntityValidationConstants;

namespace Automotive_Project.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(EmailAddressMaxLength, ErrorMessage = "Max 50 characters allowed.")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(PasswordMaxLenght, MinimumLength = PasswordMinLenght, ErrorMessage = "Max 20 or min 5 characters allowed.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
